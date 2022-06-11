using UnityEngine;

public class SlingRange : MonoBehaviour
{
    [SerializeField] private SphereCollider headCollider;
    [SerializeField] private SphereCollider rangeCollider;
    [SerializeField] private Transform arm;
    [SerializeField] private LayerMask groundLayers;


    public float Range
    {
        get
        {
            var headPosition = headCollider.transform.position;
            var armPosition = arm.position;
            var direction = headPosition - armPosition;
            var maxRange = rangeCollider.radius;

            if (Physics.SphereCast(armPosition, headCollider.radius, direction, out var hitInfo, maxRange, groundLayers))
            {
                return Mathf.Min(maxRange, hitInfo.distance);
            }

            return maxRange;
        }
    }
}