using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace TubeSystem
{
    public abstract class Tube : MonoBehaviour
    {
        private const string EventsFo = "Events";
        
        
        [Header("Values")]
        [SerializeField, Min(0f)] private float chargeDuration;


        [Foldout(EventsFo)] 
        public UnityEvent 
            onSlingEntered,
            onSlingThrown;
        
        
        private bool m_IsFilled;


        protected abstract void Charge();
        

        public bool TryEnter()
        {
            if (m_IsFilled) return false;
            
            Enter();

            return true;
        }


        private void Enter()
        {
            m_IsFilled = true;
            
            onSlingEntered?.Invoke();
            
            StartCharging();
        }

        private void StartCharging()
        {
            StartCoroutine(ChargingRoutine());
            
            IEnumerator ChargingRoutine()
            {
                yield return new WaitForSeconds(chargeDuration);
                Charge();
                Throw();
            }
        }

        private void Throw()
        {
            m_IsFilled = false;
            
            onSlingThrown?.Invoke();
        }
    }
}
