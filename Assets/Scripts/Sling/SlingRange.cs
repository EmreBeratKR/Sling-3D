using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Sling
{
    [RequireComponent(typeof(SphereCollider))]
    public class SlingRange : MonoBehaviour
    {
        public const float DistanceError = 0.01f;
    
        [Header("Event Channels")]
        [SerializeField] private VoidEventChannel slingHeadDragStart;
        [SerializeField] private VoidEventChannel slingHeadDragEnd;
        [SerializeField] private VoidEventChannel enteredStretchRange;
        [SerializeField] private VoidEventChannel exitedStretchRange;
        [SerializeField] private Vector3EventChannel slingHeadDrag;
    
        [Header("References")]
        [SerializeField] private SlingArm arm;
        [SerializeField] private SlingHead head;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private LayerMask attachSpotLayers;

        [Header("Values")]
        [SerializeField, Min(0.1f)] private float minRadius;
        [SerializeField] private bool useMinRadius;

    
        private SphereCollider rangeCollider;
        public bool IsDragging { get; private set; }


        public float Radius => rangeCollider.radius;

        public Vector3? ClosestAttachSpot
        {
            get
            {
                var origin = head.Position;
                var overlaps = Physics.OverlapSphere(origin, rangeCollider.radius, attachSpotLayers);

                if (overlaps.Length == 0) return null;

                var closestPoint = overlaps[0].ClosestPointOnBounds(origin);
                var direction = (origin - closestPoint).normalized;

                if (overlaps.Length == 1)
                {
                    return ValidateAttachSpot(origin, closestPoint + direction * DistanceError);
                }

                var closestDistance = Vector3.Distance(closestPoint, origin);

                for (int i = 1; i < overlaps.Length; i++)
                {
                    var point = overlaps[i].ClosestPointOnBounds(origin);
                    var distance = Vector3.Distance(point, origin);
                
                    if (distance >= closestDistance) continue;

                    closestPoint = point;
                    closestDistance = distance;
                }

                direction = (origin - closestPoint).normalized;
                return ValidateAttachSpot(origin, closestPoint + direction * DistanceError);
            }
        }


        private Vector3? ValidateAttachSpot(Vector3 origin, Vector3? attachSpot)
        {
            if (!attachSpot.HasValue) return null;

            var direction = attachSpot.Value - origin;
            var distance = direction.magnitude;

            var isHit = Physics.Raycast(origin, direction, distance, groundLayers);

            return isHit ? null : attachSpot;
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

            var direction = (mousePosition - armPosition).normalized;
            var isHit = Physics.SphereCast(origin, headRadius, direction, out var hitInfo, distance, groundLayers);
            var validDistance = isHit ? hitInfo.distance : Vector3.Distance(mousePosition, origin);
            validDistance = Mathf.Clamp(validDistance, 0f, Radius);

            if (!useMinRadius) return origin + direction * validDistance;
        
            var rawResult = origin + direction * validDistance;
            var rawDirection = (rawResult - armPosition).normalized;
            var rawDistance = Vector3.Distance(rawResult, armPosition);
            rawDistance = Mathf.Clamp(rawDistance, minRadius, Radius);

            return origin + rawDirection * rawDistance;
        }
    
    
    
        private void Start()
        {
            rangeCollider = GetComponent<SphereCollider>();
        }

        private void OnMouseDown()
        {
            if (!LevelSystem.IsPlaying) return;
            
            if (!arm.IsAttached && !arm.HasAttachSpotNearBy) return;

            IsDragging = true;
        
            slingHeadDragStart.RaiseEvent();
        }

        private void OnMouseDrag()
        {
            if (!LevelSystem.IsPlaying) return;
            
            if (!IsDragging) return;
        
            var mousePosition = MouseRaycaster.GetWorldPosition();
        
            if (!mousePosition.HasValue) return;

            var validPosition = ValidatePosition(mousePosition.Value);
        
            slingHeadDrag.RaiseEvent(validPosition);
        }

        private void OnMouseUp()
        {
            if (!LevelSystem.IsPlaying) return;
            
            if (!IsDragging) return;

            IsDragging = false;
        
            slingHeadDragEnd.RaiseEvent();
        }

        private void OnMouseEnter()
        {
            enteredStretchRange.RaiseEvent();
        }

        private void OnMouseExit()
        {
            exitedStretchRange.RaiseEvent();
        }

        private void Update()
        {
            FollowArm();
        }


        public void OnSlingArmAutoAttached()
        {
            IsDragging = false;
        }

        public void OnSlingArmAutoDetached()
        {
            IsDragging = false;
        }
        

        private void FollowArm()
        {
            transform.position = arm.Position;
        }
    }
}
