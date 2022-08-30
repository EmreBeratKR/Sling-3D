using UnityEngine;

namespace EnemySystem
{
    public abstract class EntityHealthBar : MonoBehaviour
    {
        public virtual void UpdateBar(float fillAmount) {}
    }
}