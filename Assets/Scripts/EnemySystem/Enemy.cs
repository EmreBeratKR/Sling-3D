using ScriptableEvents.Core.Channels;
using SoundSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemySystem
{
    public abstract class Enemy : MonoBehaviour, IDamageable, IBounce
    {
        [Header(nameof(Enemy))]
        [Header("Event Channels")] 
        [SerializeField] private VoidEventChannel goalAchieved;

        [Header("References")]
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private SfxPlayer gurpPlayer;
        [SerializeField] private AudioClip dieSound;
        
        [Header("Values")]
        [SerializeField, Min(0)] private float health;


        public Vector3 Position => transform.position;
        public bool IsDead => m_CurrentHealth <= 0;


        protected float HealthRate => m_CurrentHealth / health;
        
        
        private float m_CurrentHealth;
        protected float m_GurpTimer;
        
        
        protected virtual void OnDamaged(){}
        protected virtual void OnHealed(){}

        protected virtual void OnDead()
        {
            soundPlayer.PlayClip(dieSound);
            goalAchieved.RaiseEvent();
        }
        
        
        protected virtual void Start()
        {
            m_GurpTimer = GetGurpTimer();
            SetHealth(health);
        }

        protected virtual void Update()
        {
            TickGurpSound();
        }


        public void Damage(float damageAmount)
        {
            if (IsDead) return;
            
            m_CurrentHealth = Mathf.Max(0f, m_CurrentHealth - damageAmount);
            
            OnDamaged();
            
            if (!IsDead) return;
            
            OnDead();
        }
        
        public Vector3 CalculateDirection(Vector3 impactPoint)
        {
            var direction = impactPoint - Position;
            direction.z = 0f;
            return direction.normalized;
        }


        protected void Heal(float amount)
        {
            m_CurrentHealth = Mathf.Clamp(m_CurrentHealth + amount, 0f, health);
            
            OnHealed();
        }


        private void SetHealth(float healthAmount)
        {
            m_CurrentHealth = healthAmount;
        }

        private float GetGurpTimer()
        {
            return Random.Range(2.5f, 5f);
        }

        private void TickGurpSound()
        {
            if (IsDead) return;

            if (m_GurpTimer > 0f)
            {
                m_GurpTimer -= Time.deltaTime;
            }
            
            if (m_GurpTimer > 0f) return;

            gurpPlayer.PlayRandomClip();
            m_GurpTimer = GetGurpTimer();
        }
    }
}