using EnemySystem;
using UnityEngine;

public class BouncyHarmful : Harmful, IBounce
{
    public Vector3 CalculateDirection(Vector3 impactPoint)
    {
        var direction = impactPoint - transform.position;
        direction.z = 0f;
        return direction.normalized;
    }
}