using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace EffectSystem
{
    public abstract class Effect : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField, Min(0f)] private float duration;
        [SerializeField, Min(0f)] private float warningBlinkInterval;
        [SerializeField, Range(0f, 1f)] private float criticalRate;

        
        public bool IsActive => m_TimeLeft > 0f;


        private float CriticalTime => duration * criticalRate;
        private bool ShouldBlink => m_TimeLeft <= CriticalTime;


        private Coroutine m_ConsumeCoroutine;
        private Coroutine m_BlinkCoroutine;
        private BlinkState m_BlinkState;
        private float m_TimeLeft;
        private bool m_IsBlinking;


        protected abstract void OnActivated();
        protected abstract void OnDeactivated();

        protected abstract void OnBlinkVisible();
        protected abstract void OnBlinkInvisible();
        

        public void Activate()
        {
            if (IsActive)
            {
                m_TimeLeft = duration;
                return;
            }
            
            Activation(duration, ActivationType.Exact);
        }

        public void AddTime(float time)
        {
            if (IsActive)
            {
                m_TimeLeft += time;
                return;
            }
            
            Activation(time, ActivationType.Additive);
        }

        public void Clear()
        {
            m_TimeLeft = 0f;
        }


        private void Activation(float time, ActivationType activationType)
        {
            m_TimeLeft = activationType switch
            {
                ActivationType.Exact => time,
                ActivationType.Additive => m_TimeLeft + time,
                _ => throw new ArgumentOutOfRangeException(nameof(activationType), activationType, null)
            };
            
            OnActivated();
            StartConsuming();
        }
        
        private void Deactivate()
        {
            StopConsuming();
            StopBlinking();
            OnDeactivated();
        }
        
        private void StartConsuming()
        {
            if (!IsActive) return;

            m_ConsumeCoroutine = StartCoroutine(ConsumingRoutine());
            
            IEnumerator ConsumingRoutine()
            {
                while (true)
                {
                    yield return null;

                    m_TimeLeft = Mathf.Max(0f, m_TimeLeft - Time.deltaTime);

                    if (IsActive)
                    {
                        if (!ShouldBlink)
                        {
                            StopBlinking();
                            continue;
                        }
                        
                        StartBlinking();
                        continue;
                    }
                    
                    Deactivate();
                    yield break;
                }
            }
        }

        private void StopConsuming()
        {
            if (!IsActive) return;

            m_TimeLeft = 0f;
            
            StopCoroutine(m_ConsumeCoroutine);
        }

        private void StartBlinking()
        {
            if (!IsActive) return;
            
            if (m_IsBlinking) return;

            m_IsBlinking = true;
            m_BlinkState = BlinkState.Visible;
            
            m_BlinkCoroutine = StartCoroutine(BlinkingRoutine());
            
            IEnumerator BlinkingRoutine()
            {
                while (true)
                {
                    yield return new WaitForSeconds(warningBlinkInterval);
                    
                    InvertBlinkState();

                    if (m_BlinkState == BlinkState.Visible)
                    {
                        OnBlinkVisible();
                        continue;
                    }
                    
                    OnBlinkInvisible();
                }
            }
        }

        private void StopBlinking()
        {
            if (!m_IsBlinking) return;

            m_IsBlinking = false;
            
            OnBlinkVisible();
            StopCoroutine(m_BlinkCoroutine);
        }

        private void InvertBlinkState()
        {
            m_BlinkState = m_BlinkState == BlinkState.Visible
                ? BlinkState.Invisible
                : BlinkState.Visible;
        }
        
        
        private enum ActivationType
        {
            Exact,
            Additive
        }
        
        private enum BlinkState
        {
            Visible,
            Invisible
        }


#if UNITY_EDITOR
        
        [SerializeField, ReadOnly] private float previewCriticalTime;


        private void OnValidate()
        {
            previewCriticalTime = CriticalTime;
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TestActivate()
        {
            Activate();
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TestDeactivate()
        {
            Deactivate();
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void TestAddTime()
        {
            AddTime(1f);
        }

#endif
    }
}