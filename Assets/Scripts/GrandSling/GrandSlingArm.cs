using Handle_System;
using UnityEngine;

namespace GrandSling
{
    public class GrandSlingArm : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private GrandSlingBehaviour behaviour;
        [SerializeField] private GrandSlingHead head;
        [SerializeField] private Transform armModel;
        [SerializeField] private Rigidbody body;
        
        
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
        public bool IsAttachedToHandle => AttachedHandle;


        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void FixedUpdate()
        {
            FollowHead();
        }

        private void Update()
        {
            FollowAttachedHandle();
        }


        private void OnGrandSlingAttachedToHandle(GrandSlingBehaviour.EventResponse response)
        {
            AttachToHandle(response.attachedHandle);
        }

        private void OnGrandSlingDetachedFromHandle(GrandSlingBehaviour.EventResponse response)
        {
            AttachedHandle = null;
        }


        private void AttachToHandle(Handle handle)
        {
            AttachedHandle = handle;
        }
        
        private void FollowHead()
        {
            if (IsAttachedToHandle) return;
            
            Position = head.Position + head.transform.up * 0.6f;
        }
        
        private void FollowAttachedHandle()
        {
            if (!IsAttachedToHandle) return;

            Position = AttachedHandle.Position;
        }

        private void AddListeners()
        {
            if (behaviour)
            {
                behaviour.OnAttachedToHandle += OnGrandSlingAttachedToHandle;
                behaviour.OnDetachedFromHandle += OnGrandSlingDetachedFromHandle;
            }
        }

        private void RemoveListeners()
        {
            if (behaviour)
            {
                behaviour.OnAttachedToHandle -= OnGrandSlingAttachedToHandle;
                behaviour.OnDetachedFromHandle -= OnGrandSlingDetachedFromHandle;
            }
        }
    }
}