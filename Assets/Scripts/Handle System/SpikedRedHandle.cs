using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Handle_System
{
    public class SpikedRedHandle : Handle
    {
        [Header("References")]
        [SerializeField] private Transform spikes;
        [SerializeField] private Collider mainCollider;
        
        [Header("Values")]
        [SerializeField] private float toggleInterval;
        [SerializeField] private float toggleDuration;

        private State state;


        private void Start()
        {
            StartToggle();
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
            mainCollider.enabled = false;
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
                    mainCollider.enabled = true;
                });
        }


        private enum State
        {
            Harmless,
            Harmful
        }
    }
}