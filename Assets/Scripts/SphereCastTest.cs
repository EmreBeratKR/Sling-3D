using UnityEngine;

public class SphereCastTest : MonoBehaviour
{
    private const float DistanceError = 0.01f;
    
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private Transform target;
    [SerializeField] private float radius;
    
    private void OnDrawGizmos()
    {
        var position = transform.position;
        var targetPosition = target.position;
        var direction = (targetPosition - position).normalized;
        var distance = Vector3.Distance(targetPosition, position);
        var origin = position;


        var initialHits = Physics.OverlapSphere(position, radius, groundLayers);

        foreach (var initialHit in initialHits)
        {
            var point = initialHit.ClosestPoint(position);
            var pointDistance = Vector3.Distance(point, position);
            origin += (position - point).normalized * (radius - pointDistance + DistanceError);
        }
        
        Gizmos.DrawWireSphere(origin, radius);
        
        
        var isHit = Physics.SphereCast(origin, radius, direction, out var hitInfo, distance, groundLayers);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(origin, 0.1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(targetPosition, 0.1f);
        
        if (!isHit) return;
        
        /*if (hits.Length == 0) return;

        var closestHit = hits[0];

        for (int i = 1; i < hits.Length; i++)
        {
            var hit = hits[i];
            
            if (hit.distance >= closestHit.distance) continue;

            closestHit = hit;
        }*/
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin + direction * hitInfo.distance, radius);
    }
}
