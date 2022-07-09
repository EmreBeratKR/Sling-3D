using DG.Tweening;
using NaughtyAttributes;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Sling
{
    public class SlingBehaviour : MonoBehaviour
    {
        [Header("Event Channels")]
        [SerializeField] private VoidEventChannel levelStarted;
        [SerializeField] private VoidEventChannel levelCompleted;
        [SerializeField] private VoidEventChannel levelFailed;
        
        [Header("References")]
        [SerializeField] private GameObject[] functionalParts;
        [SerializeField] private SlingArm arm;
        [SerializeField] private SlingHead head;

        [Header("Animation Settings")]
        [SerializeField] private float portalTravelDuration;
        
        
        private void Start()
        {
            Deactivate();
        }


        public void OnPortalSpawned(Vector3 position)
        {
            transform.position = position;
            Activate();
            
            levelStarted.RaiseEvent();
        }

        public void OnPortalEnteringStart()
        {
            arm.DisablePhysics();
            head.DisablePhysics();
            
            var origin = new GameObject("Dummy").transform;
            origin.position = arm.Position;
            arm.transform.parent = origin;
            head.transform.parent = origin;

            origin.DOScale(Vector3.zero, portalTravelDuration)
                .OnComplete(() =>
                {
                    levelCompleted.RaiseEvent();
                });
        }

        public void OnSlingDied()
        {
            levelFailed.RaiseEvent();
        }

        
        [Button]
        private void Activate()
        {
            foreach (var functionalPart in functionalParts)
            {
                functionalPart.SetActive(true);
            }
        }

        [Button]
        private void Deactivate()
        {
            foreach (var functionalPart in functionalParts)
            {
                functionalPart.SetActive(false);
            }
        }
    }
}