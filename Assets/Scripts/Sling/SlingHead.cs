using Handle_System;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Sling
{
    [RequireComponent(typeof(Rigidbody))]
    public class SlingHead : MonoBehaviour
    {
        private const float StrongPullRateThreshold = 0.5f;
        private const int MidBounceLimit = 5;
        private const int LowBounceLimit = 10;

        [Header("Event Channels")]
        [SerializeField] private HandleEventChannel slimeArmAutoAttached;
        [SerializeField] private VoidEventChannel levelFailed;
        [SerializeField] private VoidEventChannel slingLostLife;
    
        [Header("References")]
        [SerializeField] private SphereCollider mainCollider;
        [SerializeField] private SlingArm arm;
        [SerializeField] private SlingRange range;
        [SerializeField] private PhysicMaterial fullBouncy;
        [SerializeField] private PhysicMaterial midBouncy;
        [SerializeField] private PhysicMaterial lowBouncy;
    
        [Header("Gravity")]
        [SerializeField] private float gravityScale;
        [SerializeField] private bool useGravity;
    
        [Header("Drag")]
        [SerializeField, Min(0f)] private float attachedDrag;
        [SerializeField, Min(0f)] private float detachedDrag;

        [Header("Force")] 
        [SerializeField] private AnimationCurve throwForceGraph;
        [SerializeField, Min(0f)] private float throwForceMultiplier;

        [Header("Values")]
        [SerializeField, Min(0f)] private float loseLifeInterval;

        private Rigidbody body;
        private int bounceCount;
        private float lastLifeLostTime;


        public float Radius => mainCollider.radius;
    

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


        private void Start()
        {
            body = GetComponent<Rigidbody>();
            body.useGravity = false;
        }

        private void FixedUpdate()
        {
            ApplyGravity();   
        }
    
        private void OnCollisionEnter(Collision collision)
        {
            bounceCount++;

            mainCollider.material = bounceCount switch
            {
                >= LowBounceLimit => lowBouncy,
                >= MidBounceLimit => midBouncy,
                _ => mainCollider.material
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LevelSystem.IsLevelEnd) return;
            
            if (other.TryGetComponent(out Handle handle))
            {
                if (handle == arm.AttachedHandle)
                {
                    if (!range.IsDragging)
                    {
                        arm.ClearHandle();
                    }
                }
                else
                {
                    handle.OnAttached();
                    slimeArmAutoAttached.RaiseEvent(handle);
                }
            }

            else if (other.TryGetComponent(out Harmful _))
            {
                TryLoseLife();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Game Area"))
            {
                if (LevelSystem.IsPlaying)
                {
                    levelFailed.RaiseEvent();
                }
            }
        }
        

        public void OnSlingHeadDragStart()
        {
            useGravity = false;
            body.drag = 0;
            body.velocity = Vector3.zero;
            bounceCount = 0;
            mainCollider.material = fullBouncy;
        }

        public void OnSlingHeadDrag(Vector3 mousePosition)
        {
            Position = mousePosition;
        }
    
        public void OnSlingHeadDragEnd()
        {
            var force = arm.Position - Position;
            var pullLength = force.magnitude;
            var pullRate = Mathf.Clamp01(pullLength / range.Radius);
            var forceAmount = throwForceGraph.Evaluate(pullRate) * throwForceMultiplier;
            var throwDirection = force.normalized;
            AddForce(throwDirection * forceAmount, ForceMode.VelocityChange);

            if (pullRate < StrongPullRateThreshold)
            {
                arm.ClearHandle();
            }
        }

        public void OnSlingArmAttached()
        {
            useGravity = false;
            body.drag = attachedDrag;
        }
        
        public void OnSlingArmDetached()
        {
            useGravity = true;
            body.drag = detachedDrag;
        }

        public void AddForce(Vector3 force)
        {
            body.AddForce(force);
        }
    
        public void AddForce(Vector3 force, ForceMode forceMode)
        {
            body.AddForce(force, forceMode);
        }

        public void EnablePhysics()
        {
            useGravity = true;
            body.isKinematic = false;
        }

        public void DisablePhysics()
        {
            useGravity = false;
            body.isKinematic = true;
        }

    
        private void ApplyGravity()
        {
            if (!useGravity) return;
        
            body.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        private bool TryLoseLife()
        {
            var elapsedTime = Time.time - lastLifeLostTime;

            if (elapsedTime < loseLifeInterval) return false;

            lastLifeLostTime = Time.time;
            slingLostLife.RaiseEvent();
            return true;
        }
    }
}
