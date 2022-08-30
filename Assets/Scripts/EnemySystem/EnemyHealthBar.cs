using UnityEngine;
using UnityEngine.UI;

namespace EnemySystem
{
    public class EnemyHealthBar : EntityHealthBar
    {
        [SerializeField] private Image bar;


        private bool IsAwake
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        

        public override void UpdateBar(float fillAmount)
        {
            base.UpdateBar(fillAmount);

            bar.fillAmount = fillAmount;
            
            TryAwake();
        }


        private void TryAwake()
        {
            if (IsAwake) return;

            IsAwake = true;
        }
    }
}