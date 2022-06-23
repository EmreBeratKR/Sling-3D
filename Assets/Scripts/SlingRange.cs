using ScriptableEvents.Core.Channels;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SlingRange : MonoBehaviour
{
    public const float DistanceError = 0.01f;
    
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannel slingHeadDragStart;
    [SerializeField] private VoidEventChannel slingHeadDragEnd;
    [SerializeField] private Vector3EventChannel slingHeadDrag;
    
    [Header("References")]
    [SerializeField] private SlingArm arm;
    [SerializeField] private SlingHead head;
    [SerializeField] private LayerMask groundLayers;

    [Header("Values")]
    [SerializeField, Min(0.1f)] private float minRadius;
    [SerializeField] private bool useMinRadius;

    
    private SphereCollider rangeCollider;
    private bool isDragging;


    public float Radius => rangeCollider.radius;

    public Vector3? ClosestAttachSpot
    {
        get
        {
            var origin = arm.Position;
            var overlaps = Physics.OverlapSphere(origin, rangeCollider.radius, groundLayers);

            if (overlaps.Length == 0) return null;

            var closestPoint = overlaps[0].ClosestPointOnBounds(origin);
            var direction = (origin - closestPoint).normalized;

            if (overlaps.Length == 1) return closestPoint + direction * DistanceError;

            var closestDistance = Vector3.Distance(closestPoint, origin);

            for (int i = 1; i < overlaps.Length; i++)
            {
                var point = overlaps[i].ClosestPoint(origin);
                var distance = Vector3.Distance(point, origin);
                
                if (distance >= closestDistance) continue;

                closestPoint = point;
                closestDistance = distance;
            }

            return closestPoint + direction * DistanceError;
        }
    }
    
    
    
    private Vector3 ValidatePosition(Vector3 mousePosition)
    {
        var armPosition = arm.Position;
        var distance = Vector3.Distance(mousePosition, armPosition);
        var origin = armPosition;
        var headRadius = head.Radius;


        var initialHits = Physics.OverlapSphere(armPosition, headRadius, groundLayers);

        foreach (var initialHit in initialHits)
        {
            var point = initialHit.ClosestPointOnBounds(armPosition);
            var pointDistance = Vector3.Distance(point, armPosition);
            origin += (armPosition - point).normalized * (headRadius - pointDistance + DistanceError);
        }

        var direction = (mousePosition - origin).normalized;
        var isHit = Physics.SphereCast(origin, headRadius, direction, out var hitInfo, distance, groundLayers);
        var validDistance = isHit ? hitInfo.distance : Vector3.Distance(mousePosition, origin);
        validDistance = Mathf.Clamp(validDistance, 0f, Radius);

        if (!useMinRadius) return armPosition + direction * validDistance;
        
        var rawResult = origin + direction * validDistance;
        var rawDirection = (rawResult - armPosition).normalized;
        var rawDistance = Vector3.Distance(rawResult, armPosition);
        rawDistance = Mathf.Clamp(rawDistance, minRadius, Radius);

        return armPosition + rawDirection * rawDistance;
    }
    
    
    
    private void Start()
    {
        rangeCollider = GetComponent<SphereCollider>();
    }

    private void OnMouseDown()
    {
        if (!arm.IsAttached && !arm.HasAttachSpotNearBy) return;

        isDragging = true;
        
        slingHeadDragStart.RaiseEvent();
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;
        
        var mousePosition = MouseRaycaster.GetWorldPosition();
        
        if (!mousePosition.HasValue) return;

        var validPosition = ValidatePosition(mousePosition.Value);
        
        slingHeadDrag.RaiseEvent(validPosition);
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;

        isDragging = false;
        
        slingHeadDragEnd.RaiseEvent();
    }

    private void Update()
    {
        FollowArm();
    }

    private void FollowArm()
    {
        transform.position = arm.Position;
    }
}
