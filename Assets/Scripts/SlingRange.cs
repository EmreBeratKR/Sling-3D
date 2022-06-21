using ScriptableEvents.Core.Channels;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SlingRange : MonoBehaviour
{
    public static float DistanceError = 0.01f;
    
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannel slingHeadDragStart;
    [SerializeField] private VoidEventChannel slingHeadDragEnd;
    [SerializeField] private Vector3EventChannel slingHeadDrag;
    
    [Header("References")]
    [SerializeField] private SlingArm arm;
    [SerializeField] private SlingHead head;
    [SerializeField] private LayerMask groundLayers;

    [Header("Values")]
    [SerializeField, Min(0f)] private float minRadius;

    
    private SphereCollider rangeCollider;


    public float Radius => rangeCollider.radius;



    private Vector3 ValidatePosition(Vector3 mousePosition)
    {
        var armPosition = arm.Position;
        var distance = Vector3.Distance(mousePosition, armPosition);
        var origin = armPosition;
        var headRadius = head.Radius;


        var initialHits = Physics.OverlapSphere(armPosition, headRadius, groundLayers);

        foreach (var initialHit in initialHits)
        {
            var point = initialHit.ClosestPoint(armPosition);
            var pointDistance = Vector3.Distance(point, armPosition);
            origin += (armPosition - point).normalized * (headRadius - pointDistance + DistanceError);
        }

        var direction = (mousePosition - origin).normalized;
        var isHit = Physics.SphereCast(origin, headRadius, direction, out var hitInfo, distance, groundLayers);
        var validDistance = isHit ? hitInfo.distance : Vector3.Distance(mousePosition, origin);
        validDistance = Mathf.Clamp(validDistance, 0f, Radius);

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
        slingHeadDragStart.RaiseEvent();
    }

    private void OnMouseDrag()
    {
        var mousePosition = MouseRaycaster.GetWorldPosition();
        
        if (!mousePosition.HasValue) return;

        var validPosition = ValidatePosition(mousePosition.Value);
        
        slingHeadDrag.RaiseEvent(validPosition);
    }

    private void OnMouseUp()
    {
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
