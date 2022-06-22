using System;
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

    private Rigidbody body;
    private bool isThrown;
    private bool isAttached;


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
        Attach(Position);
    }
    
    public void OnSlingDragEnd()
    {
        isThrown = true;
    }


    private void FollowHead()
    {
        if (isAttached) return;
        
        Position = head.Position + head.transform.up * 0.5f;
    }
    
    private void TryDetach()
    {
        if (!isThrown) return;

        if (shape.SqrArmLength > 0.25f) return;
        
        Detach();
    }
    
    private void Attach(Vector3 position)
    {
        if (isAttached) return;

        isAttached = true;
        
        transform.position = position;
        slingArmAttached.RaiseEvent();
    }

    private void Detach()
    {
        if (!isAttached) return;

        isAttached = false;
        
        isThrown = false;
        slingArmDetached.RaiseEvent();
    }
}