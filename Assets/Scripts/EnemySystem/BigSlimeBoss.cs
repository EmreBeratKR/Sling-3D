using UnityEngine;

namespace EnemySystem
{
    public class BigSlimeBoss : Boss
    {
        [Header(nameof(BigSlimeBoss))]
        [Header("Values")]
        [SerializeField] private float healPerSeconds;


        private void Update()
        {
            TickHealing();
        }


        protected override void OnHealed()
        {
            base.OnHealed();
            
            healthBar.UpdateBar(HealthRate);
        }


        private void TickHealing()
        {
            if (IsDead) return;
            
            var amount = Time.deltaTime * healPerSeconds;
            Heal(amount);
        }
    }
}