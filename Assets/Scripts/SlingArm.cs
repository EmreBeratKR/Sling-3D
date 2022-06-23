using ScriptableEvents.Core.Channels;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlingArm : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannel slingArmAttached;
    [SerializeField] private VoidEventChannel slingArmDetached;
    
    [Header("References")]
    [SerializeField] private SlingShape shape;
    [SerializeField] private SlingHead head;
    [SerializeField] private SlingRange range;

    private Rigidbody body;
    private bool isThrown;


    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    public Vector3 EulerAngles
    {
        get => transform.eulerAngles;
        set => transform.eulerAngles = value;
    }

    public Vector3 LocalScale
    {
        get => transform.localScale;
        set => transform.localScale = value;
    }
    
    public bool IsAttached { get; private set; }

    public bool HasAttachSpotNearBy => range.ClosestAttachSpot.HasValue;


    private void Start()
    {
        body = GetComponent<Rigidbody>();
        Attach(transform.position);
    }

    private void FixedUpdate()
    {
        FollowHead();
    }

    private void Update()
    {
        TryDetach();
    }


    public void OnSlingDragStart()
    {
        var attachSpot = range.ClosestAttachSpot;
        
        if (!attachSpot.HasValue) return;
        
        Attach(attachSpot.Value);
    }
    
    public void OnSlingDragEnd()
    {
        isThrown = true;
    }


    private void FollowHead()
    {
        if (IsAttached) return;
        
        Position = head.Position + head.transform.up * 0.5f;
    }
    
    private void TryDetach()
    {
        if (!isThrown) return;

        if (shape.SqrArmLength > 1f) return;
        
        Detach();
    }
    
    private void Attach(Vector3 position)
    {
        if (IsAttached) return;

        IsAttached = true;
        
        transform.position = position;
        slingArmAttached.RaiseEvent();
    }

    private void Detach()
    {
        if (!IsAttached) return;

        IsAttached = false;
        
        isThrown = false;
        slingArmDetached.RaiseEvent();
    }
}