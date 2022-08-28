using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Handle_System
{
    public class SpikedInfectableHandle : InfectableHandle
    {
        [Header("References")]
        [SerializeField] private Transform spikes;

        [Header("Values")]
        [SerializeField] private float toggleInterval;
        [SerializeField] private float toggleDuration;

        private State state;


        private void Start()
        {
            StartToggle();
        }


        protected override void OnAttached()
        {
            base.OnAttached();
            
            StopToggle();
        }


        private void StartToggle()
        {
            StartCoroutine(Toggling());
            
            IEnumerator Toggling()
            {
                var interval = new WaitForSeconds(toggleInterval);

                while (true)
                {
                    yield return interval;
                    ToggleSpikes();
                }
            }
        }

        private void StopToggle()
        {
            StopAllCoroutines();
        }

        private void ToggleSpikes()
        {
            state = state == State.Harmless ? State.Harmful : State.Harmless;

            if (state == State.Harmful)
            {
                EnableSpikes();
                return;
            }

            DisableSpikes();
        }

        private void EnableSpikes()
        {
            spikes.gameObject.SetActive(true);
            DisableHandle();
            spikes.DOScale(Vector3.one, toggleDuration)
                .SetEase(Ease.OutSine);
        }
        
        private void DisableSpikes()
        {
            spikes.DOScale(Vector3.zero, toggleDuration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    spikes.gameObject.SetActive(false);
                    EnableHandle();
                });
        }


        private enum State
        {
            Harmless,
            Harmful
        }
    }
}