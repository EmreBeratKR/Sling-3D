using System.Collections;
using PathSystem;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Handle_System
{
    public class WobblerHandle : Handle
    {
        [Header("Event Channels")]
        [SerializeField] private HandleEventChannel onHandleWobbling;
        
        [Header("References")]
        [SerializeField] private PathMover pathMover;
        
        [Header("Values")]
        [SerializeField, Min(0f)] private float interval;
        [SerializeField] private bool looping;
        [SerializeField] private bool playOnStart;


        private void Start()
        {
            if (playOnStart)
            {
                StartWobble();
            }
        }
        

        public void OnWobbleComplete()
        {
            TryRestartWobble();
        }

        public void OnWobbling()
        {
            onHandleWobbling.RaiseEvent(this);
        }
        

        public void StartWobble()
        {
            pathMover.Play();
        }

        public void StopWobble()
        {
            pathMover.Stop();
        }


        private void TryRestartWobble()
        {
            if (!looping) return;

            StartCoroutine(RestartRoutine());
            
            IEnumerator RestartRoutine()
            {
                yield return new WaitForSeconds(interval);
                
                if (!looping) yield break;
                
                StartWobble();
            }
        }
    }
}