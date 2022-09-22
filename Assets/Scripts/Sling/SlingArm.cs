using Handle_System;
using ScriptableEvents.Core.Channels;
using TubeSystem;
using UnityEngine;

namespace Sling
{
    [RequireComponent(typeof(Rigidbody))]
    public class SlingArm : MonoBehaviour
    {
        [Header("Event Channels")]
        [SerializeField] private VoidEventChannel slingArmAttached;
        [SerializeField] private VoidEventChannel slingArmDetached;
        [SerializeField] private VoidEventChannel slingArmAutoDetached;
    
        [Header("References")]
        [SerializeField] private SlingShape shape;
        [SerializeField] private SlingHead head;
        [SerializeField] private SlingRange range;
        [SerializeField] private Transform armModel;

        private Rigidbody body;
        private bool isThrown;
        private SlingAction lastAction;


        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    
        public Vector3 EulerAngles
        {
            get => transform.eulerAngles;
            set => transform.eulerAngles = value;
        }

        public Vector3 LocalScale
        {
            get => armModel.localScale;
            set => armModel.localScale = value;
        }
    
        public Handle AttachedHandle { get; private set; }
        public bool IsAttached { get; private set; }
        
        public bool IsAttachedToHandle => AttachedHandle != null;
        public bool HasAttachSpotNearBy => range.ClosestAttachSpot.HasValue;


        private void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            Tube.OnSlingEntered += OnSlingEnteredTube;
            SlingBehaviour.OnThrownByThrowerHandle += OnSlingThrownByThrowerHandle;
        }

        private void OnDisable()
        {
            Tube.OnSlingEntered -= OnSlingEnteredTube;
            SlingBehaviour.OnThrownByThrowerHandle -= OnSlingThrownByThrowerHandle;
        }

        private void FixedUpdate()
        {
            FollowHead();
        }

        private void Update()
        {
            TryDetach();
            FollowHandle();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Harmful _))
            {
                head.TryLoseLife();
            }
        }


        public void OnSlingDragStart()
        {
            if (IsAttachedToHandle) return;
            
            var attachSpot = range.ClosestAttachSpot;
        
            if (!attachSpot.HasValue) return;
        
            Attach(attachSpot.Value);
        }
    
        public void OnSlingDragEnd()
        {
            lastAction = SlingAction.Throw;
            isThrown = true;
        }

        public void OnAutoAttached(Handle handle)
        {
            lastAction = SlingAction.AutoAttach;
            AttachedHandle = handle;
            Attach(handle.Position);
        }

        public void OnHandleGrabbed(Handle grabbedHandle)
        {
            TryAutoDetach(grabbedHandle);
        }

        public void OnHandleWobbling(Handle wobblingHandle)
        {
            if (AttachedHandle == wobblingHandle)
            {
                Vector3 randomForce = Random.insideUnitCircle.normalized * 20f;
                head.AddForce(randomForce, ForceMode.VelocityChange);
            }
            
            TryAutoDetach(wobblingHandle);
        }

        public void OnAttachedToDirtyHandle(Handle dirtyHandle)
        {
            TryAutoDetach(dirtyHandle);
        }

        private void OnSlingEnteredTube(EnterTubeResponse response)
        {
            AutoDetach();
        }

        private void OnSlingThrownByThrowerHandle()
        {
            AttachedHandle.IgnoreFor(0.1f);
            AutoDetach();
        }

        public bool IsAttachedTo(Handle handle)
        {
            return AttachedHandle == handle;
        }
        
        public void EnablePhysics()
        {
            body.isKinematic = false;
        }

        public void DisablePhysics()
        {
            body.isKinematic = true;
        }
        
        public void ClearHandle()
        {
            if (lastAction != SlingAction.Throw) return;
            
            AttachedHandle = null;
        }

        public void ForceFollowHead()
        {
            Position = head.Position + head.transform.up * 0.6f;
        }
        
        public void ForceAutoDetach()
        {
            AutoDetach();
        }
        
        
        private void FollowHead()
        {
            if (IsAttached) return;
        
            Position = head.Position + head.transform.up * 0.6f;
        }

        private void FollowHandle()
        {
            if (!IsAttachedToHandle) return;

            Position = AttachedHandle.Position;
        }

        private void TryAutoDetach(Handle handle)
        {
            if (AttachedHandle != handle) return;
            
            AutoDetach();
        }

        private void TryDetach()
        {
            if (!isThrown) return;

            if (shape.SqrArmLength > 1.5f) return;
            
            Detach();
        }
    
        private void Attach(Vector3 position)
        {
            IsAttached = true;
        
            transform.position = position;
            slingArmAttached.RaiseEvent();
        }

        private void AutoDetach()
        {
            if (!IsAttached) return;
            
            Detach();

            AttachedHandle = null;
            
            slingArmAutoDetached.RaiseEvent();
        }

        private void Detach()
        {
            if (!IsAttached) return;

            IsAttached = false;
        
            isThrown = false;
            slingArmDetached.RaiseEvent();
        }
        
        
        private enum SlingAction
        {
            None,
            Throw,
            AutoAttach
        }
    }
}