using ScriptableEvents.Core.Channels;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SlingRange : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannel slingHeadDragStart;
    [SerializeField] private VoidEventChannel slingHeadDragEnd;
    [SerializeField] private Vector3EventChannel slingHeadDrag;
    
    [Header("References")]
    [SerializeField] private SlingArm arm;

    
    private SphereCollider rangeCollider;


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
        
        slingHeadDrag.RaiseEvent(mousePosition.Value);
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
