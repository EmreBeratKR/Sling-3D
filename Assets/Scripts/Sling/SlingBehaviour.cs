using DG.Tweening;
using Handle_System;
using NaughtyAttributes;
using ScriptableEvents.Core.Channels;
using TubeSystem;
using UnityEngine;
using UnityEngine.Events;

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


        public static UnityAction OnThrownByThrowerHandle;
        
        
        private void Start()
        {
            Deactivate();
        }

        private void OnEnable()
        {
            Tube.OnSlingEntered += OnSlingEnteredTube;
            Tube.OnSlingThrown += OnSlingThrownFromTube;
            ThrowerHandle.OnThrow += OnThrowerHandleThrow;
        }

        private void OnDisable()
        {
            Tube.OnSlingEntered -= OnSlingEnteredTube;
            Tube.OnSlingThrown -= OnSlingThrownFromTube;
            ThrowerHandle.OnThrow -= OnThrowerHandleThrow;
        }

        private void OnSlingEnteredTube(EnterTubeResponse response)
        {
            var enteredTube = response.enteredTube;

            Move(enteredTube.ChargingSpot);
            Deactivate();
        }

        private void OnSlingThrownFromTube(ThrownFromTubeResponse response)
        {
            var thrownFrom = response.thrownFrom;
            var throwForce = thrownFrom.ThrowForce - head.Velocity;
            Activate();
            head.AddForce(throwForce, ForceMode.VelocityChange);
        }

        private void OnThrowerHandleThrow(ThrowerHandleThrowResponse response)
        {
            var throwerHandle = response.throwerHandle;
            
            if (!arm.IsAttachedTo(throwerHandle)) return;

            var throwForce = response.force - head.Velocity;
            Debug.Log($"{response.force} => {throwForce}");
            head.AddForce(throwForce, ForceMode.VelocityChange);
            
            OnThrownByThrowerHandle?.Invoke();
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


        private void Move(Vector3 position)
        {
            head.Position = position;
            arm.ForceFollowHead();
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