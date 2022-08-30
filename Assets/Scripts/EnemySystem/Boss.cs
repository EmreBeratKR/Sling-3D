using DG.Tweening;
using UnityEngine;

namespace EnemySystem
{
    public abstract class Boss : Enemy
    {
        [Header("References")] 
        [SerializeField] private EnemyHealthBar healthBar;
        [SerializeField] private Collider trigger;
        [SerializeField] private Transform main;

        [Header("Values")]
        [SerializeField] private float dieAnimationDuration;


        protected override void OnDamaged()
        {
            base.OnDamaged();
    
            healthBar.UpdateBar(HealthRate);
        }

        protected override void OnDead()
        {
            base.OnDead();

            DisableInteraction();
            ShrinkDisappear();
        }


        private void DisableInteraction()
        {
            Destroy(trigger);
        }

        private void ShrinkDisappear()
        {
            main.DOScale(Vector3.zero, dieAnimationDuration)
                .SetEase(Ease.InBack);
        }
    }
}