using CollectableSystem;
using Data_Container;
using EffectSystem;
using EnemySystem;
using Handle_System;
using ScriptableEvents.Core.Channels;
using SoundSystem;
using TubeSystem;
using UnityEngine;

namespace Sling
{
    [RequireComponent(typeof(Rigidbody))]
    public class SlingHead : MonoBehaviour
    {
        private const float StrongPullRateThreshold = 0.5f;
        private const float KnockBackForce = 10f;
        public const int MidBounceLimit = 5;
        public const int LowBounceLimit = 10;
        private const float Damage = 1f;
        
        private const float StretchSoundSqrThreshold = 0.0001f;

        
        [Header("Event Channels")]
        [SerializeField] private HandleEventChannel slimeArmAutoAttached;
        [SerializeField] private HandleEventChannel attachedToDirtyHandle;
        [SerializeField] private VoidEventChannel levelFailed;
        [SerializeField] private VoidEventChannel slingLostLife;
    
        [Header("References")]
        [SerializeField] private SphereCollider mainCollider;
        [SerializeField] private SlingBehaviour behaviour;
        [SerializeField] private SlingArm arm;
        [SerializeField] private SlingRange range;
        [SerializeField] private SlingSound sound;
        [SerializeField] private SlimeEffect slimeEffect;
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


        public Vector3 Velocity => body.velocity;
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
            sound.PlayBounce();

            mainCollider.material = bounceCount switch
            {
                >= LowBounceLimit => lowBouncy,
                >= MidBounceLimit => midBouncy,
                _ => mainCollider.material
            };

            var other = collision.collider;
            
            if (other.TryGetComponent(out AutoDetacher _))
            {
                arm.ForceAutoDetach();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LevelSystem.IsLevelEnd) return;
            
            if (other.TryGetComponent(out IDamageable damageable))
            {
                if (TryDamage(damageable))
                {
                    
                }
            }

            if (other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect(behaviour);
            }
            
            if (other.TryGetComponent(out GameAreaBorder _))
            {
                ExitedFromGameArea();
            }
            
            if (other.TryGetComponent(out IBounce bounce))
            {
                KnockBackFromBounce(bounce);
            }
            
            else if (other.TryGetComponent(out Handle handle))
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
                    if (handle.TryAttach())
                    {
                        slimeArmAutoAttached.RaiseEvent(handle);

                        TryCleanHandle(handle);
                    }
                }
            }

            else if (other.TryGetComponent(out TubeEntrance tubeEntrance))
            {
                TryEnterTube(tubeEntrance);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Harmful _))
            {
                if (other.TryGetComponent(out Enemy enemy))
                {
                    TryLoseLife();
                }

                else
                {
                    if (TryLoseLife(true))
                    {
                        slimeEffect.Clear();
                    }
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
            var sqrDistance = Vector3.SqrMagnitude(Position - mousePosition);
            Position = mousePosition;

            if (sqrDistance > StretchSoundSqrThreshold)
            {
                sound.PlayStretch();
            }
        }
    
        public void OnSlingHeadDragEnd()
        {
            var pullForce = arm.Position - Position;
            Fling(pullForce);
            sound.PlayFling();
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

        public bool TryLoseLife(bool ignoreEffects = false)
        {
            if (slimeEffect.IsActive && !ignoreEffects) return false;
            
            var elapsedTime = Time.time - lastLifeLostTime;

            if (elapsedTime < loseLifeInterval) return false;

            lastLifeLostTime = Time.time;
            slingLostLife.RaiseEvent();
            sound.PlayTakeDamage();
            return true;
        }

        
        private void ApplyGravity()
        {
            if (!useGravity) return;
        
            body.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
        }

        private void Fling(Vector3 pullForce)
        {
            var pullLength = pullForce.magnitude;
            var pullRate = Mathf.Clamp01(pullLength / range.Radius);
            var forceAmount = throwForceGraph.Evaluate(pullRate) * throwForceMultiplier;
            var throwDirection = pullForce.normalized;
            AddForce(throwDirection * forceAmount, ForceMode.VelocityChange);

            if (pullRate < StrongPullRateThreshold)
            {
                arm.ClearHandle();
            }
        }

        private void TryCleanHandle(Handle handle)
        {
            if (handle is not ICleanable cleanable) return;
            
            if (!cleanable.IsDirty) return;
            
            if (slimeEffect.IsActive)
            {
                cleanable.Clean();
            }

            else
            {
                attachedToDirtyHandle.RaiseEvent(handle);
            }
        }

        private bool TryDamage(IDamageable damageable)
        {
            if (range.IsDragging) return false;
            
            if (!slimeEffect.IsActive) return false;

            /*if (damageable is Enemy enemy)
            {
                KnockBackFromBounce(enemy);
            }*/
            
            damageable.Damage(Damage);

            return true;
        }
        
        private void KnockBackFromBounce(IBounce bounce)
        {
            var direction = bounce.CalculateDirection(transform.position);
            direction.z = 0f;
            var force = direction.normalized * KnockBackForce - Velocity;
            AddForce(force, ForceMode.VelocityChange);
            sound.PlayBounce();
        }

        private void TryEnterTube(TubeEntrance tubeEntrance)
        {
            if (range.IsDragging) return;
            
            if (tubeEntrance.TryEnter())
            {
                    
            }
        }

        private void ExitedFromGameArea()
        {
            levelFailed.RaiseEvent();
            sound.PlayTakeDamage();
        }
    }
}
