using System;
using DG.Tweening;
using Handle_System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace LeverSystem
{ 
    public abstract class Lever : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform pull;

        [Header("Values")]
        [SerializeField, Min(0f)] private float pullSpeed;
        [SerializeField, Range(0f, 90f)] private float pullAngle;
        [SerializeField] private bool invert;
    
        [Header("Events")]
        public UnityEvent<LeverToggleResponse> onToggled;

        
        private float PullUpAngle => pullAngle;
        private float PullDownAngle => -pullAngle;


        private Tween m_PullTween;
        private bool m_IsOn;

    
#if UNITY_EDITOR
    
        private void OnValidate()
        {
            var pullEulerAngles = pull.eulerAngles;
            pullEulerAngles.x = InvertableAngle(PullUpAngle);
            pull.eulerAngles = pullEulerAngles;
        }
    
#endif

    
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Toggle()
        {
            if (m_IsOn)
            {
                ToggleOff();
                return;
            }
        
            ToggleOn();
        }
    
    
        private void ToggleOn()
        {
            if (m_IsOn) return;

            m_IsOn = true;

            var response = new LeverToggleResponse
            {
                isOn = true
            };
        
            Pull(response, InvertableAngle(PullDownAngle));
        }

        private void ToggleOff()
        {
            if (!m_IsOn) return;

            m_IsOn = false;
        
            var response = new LeverToggleResponse
            {
                isOn = false
            };
        
            Pull(response, InvertableAngle(PullUpAngle));
        }

        private float InvertableAngle(float angle)
        {
            return angle * (invert ? -1f : 1f);
        }

        private void Pull(LeverToggleResponse toggleResponse, float angle)
        {
            m_PullTween?.Kill();
            m_PullTween = pull.DORotate(Vector3.right * angle, pullSpeed);

            m_PullTween
                .SetSpeedBased()
                .OnStart(() =>
                {
                    onToggled?.Invoke(toggleResponse);
                });
        }
    
    
        [Serializable]
        public struct LeverToggleResponse
        {
            public bool isOn;
        }
    }
}
