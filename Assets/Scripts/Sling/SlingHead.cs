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
        private const int MidBounceLimit = 5;
        private const int LowBounceLimit = 10;
        private const float Damage = 1f;
        
        private const float StretchSoundSqrThreshold = 0.0001f;

        
        [Header("Event Channels")]
        [SerializeField] private HandleEventChannel slimeArmAutoAttached;
        [SerializeField] private HandleEventChannel attachedToDirtyHandle;
        [SerializeField] private VoidEventChannel levelFailed;
        [SerializeField] private VoidEventChannel slingLostLife;
    
        [Header("References")]
        [SerializeField] private SphereCollider mainCollider;
        [SerializeField] private SlingArm arm;
        [SerializeField] private SlingRange range;
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

        [Header("SFX")] 
        [SerializeField] private AudioClipContainer stretchClipContainer;
        [SerializeField] private SoundPlayer sfxStretch;
        [SerializeField] private AudioClipContainer takeDamageClipContainer;
        [SerializeField] private SoundPlayer sfxTakeDamage;


        private AudioClip RandomStretchAudioClip => stretchClipContainer.Random;
        private AudioClip RandomTakeDamageAudioClip => takeDamageClipContainer.Random;
        

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
                TryDamage(damageable);
            }
            
            if (other.TryGetComponent(out GameAreaBorder _))
            {
                levelFailed.RaiseEvent();
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
                    KnockBackFromEnemy(enemy);
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
                sfxStretch.TryPlayClip(RandomStretchAudioClip);
            }
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

        public void KnockBackFromEnemy(Enemy enemy)
        {
            var direction = transform.position - enemy.Position;
            direction.z = 0f;
            var force = direction.normalized * KnockBackForce - Velocity;
            AddForce(force, ForceMode.VelocityChange);
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

        public bool TryLoseLife(bool ignoreEffects = false)
        {
            if (slimeEffect.IsActive && !ignoreEffects) return false;
            
            var elapsedTime = Time.time - lastLifeLostTime;

            if (elapsedTime < loseLifeInterval) return false;

            lastLifeLostTime = Time.time;
            slingLostLife.RaiseEvent();
            sfxTakeDamage.PlayClip(RandomTakeDamageAudioClip);
            return true;
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

        private void TryDamage(IDamageable damageable)
        {
            if (range.IsDragging) return;
            
            if (!slimeEffect.IsActive) return;

            if (damageable is Enemy enemy)
            {
                KnockBackFromEnemy(enemy);
            }
            
            damageable.Damage(Damage);
        }

        private void TryEnterTube(TubeEntrance tubeEntrance)
        {
            if (range.IsDragging) return;
            
            if (tubeEntrance.TryEnter())
            {
                    
            }
        }
    }
}
