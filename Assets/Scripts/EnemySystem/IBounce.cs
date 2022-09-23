using UnityEngine;

namespace EnemySystem
{
    public interface IBounce
    {
        Vector3 CalculateDirection(Vector3 impactPoint);
    }
}