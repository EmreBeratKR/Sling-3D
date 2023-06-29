using System;
using DG.Tweening;
using NaughtyAttributes;
using SoundSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Handle_System
{
    public class ThrowerHandle : Handle
    {
        [Header("References")]
        [SerializeField] private Transform endPointAnchor;
        [SerializeField] private Transform model;
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private AudioClip throwSound;

        [Header("Values")] 
        [SerializeField, Min(0f)] private float throwCooldown;
        [SerializeField, Min(0f)] private float throwSpeed;
        [SerializeField, Min(0f)] private float reloadCooldown;
        [SerializeField, Min(0f)] private float reloadSpeed;
        [SerializeField, Min(0f)] private float force;
        [SerializeField] private bool activeOnStart;
        [SerializeField] private bool looping;


        public static UnityAction<ThrowerHandleThrowResponse> OnThrow;


        private Vector3 EndPoint => endPointAnchor.position;


        private Vector3 m_Direction;


        private void Start()
        {
            LookAtEndPoint();
            SetDirection();
            TryAutoStart();
        }


        private void LookAtEndPoint()
        {
            var delta = EndPoint - model.position;
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            model.eulerAngles = Vector3.forward * angle;
        }

        private void SetDirection()
        {
            m_Direction = (EndPoint - transform.position).normalized;
        }

        private void TryAutoStart()
        {
            if (!activeOnStart) return;
            
            StartThrowing();
        }
        
        private void StartThrowing()
        {
            transform.DOMove(EndPoint, throwSpeed)
                .OnStart(() =>
                {
                    soundPlayer.PlayClip(throwSound);
                })
                .SetDelay(throwCooldown)
                .SetEase(Ease.OutExpo)
                .SetSpeedBased()
                .OnComplete(() =>
                {
                    var response = new ThrowerHandleThrowResponse
                    {
                        force = force * m_Direction,
                        throwerHandle = this
                    };
                    
                    OnThrow?.Invoke(response);
                    StartReloading();
                });
        }

        private void StartReloading()
        {
            transform.DOLocalMove(Vector3.zero, reloadSpeed)
                .SetDelay(reloadCooldown)
                .SetEase(Ease.OutSine)
                .SetSpeedBased()
                .OnComplete(() =>
                {
                    if (looping)
                    {
                        StartThrowing();
                    }
                });
        }


#if UNITY_EDITOR

        [Button]
        private void SetRotation()
        {
            LookAtEndPoint();
        }

#endif
    }
    
    
    [Serializable]
    public struct ThrowerHandleThrowResponse
    {
        public ThrowerHandle throwerHandle;
        public Vector3 force;
    }
}