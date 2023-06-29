using System.Collections;
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
        [SerializeField] private SlingRange range;
        [SerializeField] private GameObject aliveVisual;
        [SerializeField] private GameObject deadVisual;

        [Header("Animation Settings")]
        [SerializeField] private float portalTravelDuration;


        public static UnityAction OnThrownByThrowerHandle;

        public UnityEvent OnEnterPortalComplete;
        
        
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
            range.InputEnabled = false;
            
            var origin = new GameObject("Dummy").transform;
            origin.position = arm.Position;
            arm.transform.parent = origin;
            head.transform.parent = origin;

            origin.DOScale(Vector3.zero, portalTravelDuration)
                .OnComplete(() =>
                {
                    OnEnterPortalComplete?.Invoke();
                    levelCompleted.RaiseEvent();
                });
        }

        public void OnSlingDied()
        {
            PlayDeath();
            levelFailed.RaiseEvent();
        }


        private void PlayDeath()
        {
            aliveVisual.SetActive(false);
            deadVisual.SetActive(true);
            
            arm.DisablePhysics();
            head.DisablePhysics();
            arm.GetComponent<Collider>().enabled = false;
            head.GetComponent<Collider>().enabled = false;
            
            arm.enabled = false;
            head.enabled = false;
            range.enabled = false;
            range.InputEnabled = false;
            Destroy(range);
            GetComponentInChildren<Spring>().enabled = false;
            GetComponentInChildren<SlingShape>().enabled = false;
            GetComponentInChildren<SlingShape>().UpdateArmLength(1f);
            
            var origin = new GameObject("Dummy").transform;
            origin.position = head.Position;
            arm.transform.parent = origin;
            head.transform.parent = origin;

            origin.DOMove(origin.position + Vector3.up * 0.5f, 0.25f)
                .SetEase(Ease.OutSine);
            origin.DOMove(new Vector3(origin.position.x, -15f, 0f), 1f)
                .SetDelay(0.25f)
                .SetEase(Ease.InSine);
            origin.DOScale(Vector3.one * 7f, 1f);
            origin.DORotate(Vector3.forward * (179f * 1f), 0.5f);
            origin.DORotate(Vector3.forward * (179f * 2f), 0.5f)
                .SetDelay(0.5f);
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



#if UNITY_EDITOR

        public void Developer_Panel_CompleteLevel()
        {
            levelCompleted.RaiseEvent();
        }
        
#endif
    }
}