using UnityEngine;

namespace Goal_System
{
    public abstract class Goal : MonoBehaviour
    {
        public abstract bool IsAchieved { get; }
    }
}
