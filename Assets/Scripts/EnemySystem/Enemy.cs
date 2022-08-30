using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace EnemySystem
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        [Header("Event Channels")] 
        [SerializeField] private VoidEventChannel goalAchieved;
        
        [Header("Values")]
        [SerializeField, Min(0)] private int health;


        public Vector3 Position => transform.position;
        public bool IsDead => m_CurrentHealth <= 0;


        protected float HealthRate => (float) m_CurrentHealth / health;
        
        
        private int m_CurrentHealth;
        
        
        protected virtual void OnDamaged(){}

        protected virtual void OnDead()
        {
            goalAchieved.RaiseEvent();
        }
        
        
        private void Start()
        {
            SetHealth(health);
        }


        public void Damage(int damageAmount)
        {
            if (IsDead) return;
            
            m_CurrentHealth = Mathf.Max(0, m_CurrentHealth - damageAmount);
            
            OnDamaged();
            
            if (!IsDead) return;
            
            OnDead();
        }


        private void SetHealth(int healthAmount)
        {
            m_CurrentHealth = healthAmount;
        }
    }
}