using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace EnemySystem
{
    public abstract class Enemy : MonoBehaviour, IDamageable, IBounce
    {
        [Header(nameof(Enemy))]
        [Header("Event Channels")] 
        [SerializeField] private VoidEventChannel goalAchieved;
        
        [Header("Values")]
        [SerializeField, Min(0)] private float health;


        public Vector3 Position => transform.position;
        public bool IsDead => m_CurrentHealth <= 0;


        protected float HealthRate => m_CurrentHealth / health;
        
        
        private float m_CurrentHealth;
        
        
        protected virtual void OnDamaged(){}
        protected virtual void OnHealed(){}

        protected virtual void OnDead()
        {
            goalAchieved.RaiseEvent();
        }
        
        
        private void Start()
        {
            SetHealth(health);
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
    }
}