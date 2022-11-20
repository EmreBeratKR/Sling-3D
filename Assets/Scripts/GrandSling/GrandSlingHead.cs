using System;
using Handle_System;
using Sling;
using UnityEngine;

namespace GrandSling
{
    public class GrandSlingHead : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private GrandSlingBehaviour behaviour;
        [SerializeField] private SphereCollider mainCollider;
        [SerializeField] private PhysicMaterial fullBouncy;
        [SerializeField] private PhysicMaterial midBouncy;
        [SerializeField] private PhysicMaterial lowBouncy;
        [SerializeField] private GrandSlingArm arm;
        [SerializeField] private SlingSound sound;
        [SerializeField] private Rigidbody body;

        [Header("Drag")]
        [SerializeField] private float attachedDrag;
        [SerializeField] private float detachedDrag;
        
        [Header("Values")]
        [SerializeField] private float gravityScale;
        [SerializeField] private bool useGravity;


        private int m_BounceCount;
        

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
            ApplyGravity();
        }

        private void OnCollisionEnter(Collision collision)
        {
            m_BounceCount++;
            sound.PlayBounce();

            mainCollider.material = m_BounceCount switch
            {
                >= SlingHead.LowBounceLimit => lowBouncy,
                >= SlingHead.MidBounceLimit => midBouncy,
                _ => mainCollider.material
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            TryAttachHandle(other);
        }


        private void OnGrandSlingAttachedToHandle(GrandSlingBehaviour.EventResponse response)
        {
            useGravity = false;
            body.drag = attachedDrag;
        }

        private void OnGrandSlingDetachedFromHandle(GrandSlingBehaviour.EventResponse response)
        {
            useGravity = true;
            body.drag = detachedDrag;
        }


        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            body.AddForce(force, forceMode);
        }
        
        
        private void ApplyGravity()
        {
            if (!useGravity) return;
            
            body.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        private void TryAttachHandle(Collider other)
        {
            if (!other.TryGetComponent(out Handle handle)) return;
            
            if (!handle.TryAttach()) return;
            
            var response = new GrandSlingBehaviour.EventResponse()
            {
                attachedHandle = handle
            };
                    
            behaviour.OnAttachedToHandle?.Invoke(response);
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