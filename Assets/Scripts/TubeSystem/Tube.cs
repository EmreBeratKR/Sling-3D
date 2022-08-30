using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace TubeSystem
{
    public abstract class Tube : MonoBehaviour
    {
        private const string EventsFo = "Events";


        [Header("References")]
        [SerializeField] private Transform chargingAnchor;

        [Header("Values")] 
        [SerializeField, Min(0f)] private float throwForce;
        [SerializeField, Min(0f)] private float chargeDuration;
        [SerializeField, Min(0f)] private float reloadCooldown;


        public static UnityAction<EnterTubeResponse> OnSlingEntered;
        public static UnityAction<ThrownFromTubeResponse> OnSlingThrown;
        

        [Foldout(EventsFo)] 
        public UnityEvent
            onReloaded;


        public Vector3 ChargingSpot => chargingAnchor.position;
        public Vector3 ThrowForce => chargingAnchor.up * throwForce;
        

        private bool CanEnter => m_State == TubeState.Idle;
        private bool CanStartCharging => m_State == TubeState.Filled;
        private bool CanBeCharged => m_State == TubeState.Charging;
        private bool CanThrow => m_State == TubeState.Charged;
        private bool CanStartReloading => m_State == TubeState.Throw;
        
        
        private TubeState m_State;


        public bool TryEnter()
        {
            if (!CanEnter) return false;
            
            Enter();

            return true;

            void Enter()
            {
                m_State = TubeState.Filled;

                var response = new EnterTubeResponse
                {
                    enteredTube = this
                };
            
                OnSlingEntered?.Invoke(response);
            
                StartCharging();
            }
        }

        
        protected virtual void Charge()
        {
            if (!CanBeCharged) return;
            
            m_State = TubeState.Charged;
        }
        

        private void StartCharging()
        {
            if (!CanStartCharging) return;
            
            StartCoroutine(ChargingRoutine());
            
            IEnumerator ChargingRoutine()
            {
                m_State = TubeState.Charging;
                
                yield return new WaitForSeconds(chargeDuration);
                
                Charge();
                Throw();
                StartReloading();
            }
        }

        private void Throw()
        {
            if (!CanThrow) return;
            
            m_State = TubeState.Throw;

            var response = new ThrownFromTubeResponse
            {
                thrownFrom = this
            };
            
            OnSlingThrown?.Invoke(response);
        }

        private void StartReloading()
        {
            if (!CanStartReloading) return;

            StartCoroutine(ReloadingRoutine());

            IEnumerator ReloadingRoutine()
            {
                m_State = TubeState.Reloading;

                yield return new WaitForSeconds(reloadCooldown);
                
                Reload();
            }

            void Reload()
            {
                m_State = TubeState.Idle;
                
                onReloaded?.Invoke();
            }
        }


        private enum TubeState
        {
            Idle,
            Filled,
            Charging,
            Charged,
            Throw,
            Reloading
        }
    }


    [Serializable]
    public struct EnterTubeResponse
    {
        public Tube enteredTube;
    }
    
    [Serializable]
    public struct ThrownFromTubeResponse
    {
        public Tube thrownFrom;
    }
}
