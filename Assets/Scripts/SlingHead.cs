using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlingHead : MonoBehaviour
{
    [SerializeField] private SphereCollider mainCollider;
    [SerializeField] private float gravityScale;
    [SerializeField] private bool useGravity;
    
    private Rigidbody body;
    private float drag;


    public float Radius => mainCollider.radius;
    

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


    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        drag = body.drag;
    }

    private void FixedUpdate()
    {
        ApplyGravity();   
    }


    public void OnSlingHeadDragStart()
    {
        useGravity = false;
        body.drag = 0;
        body.velocity = Vector3.zero;
    }

    public void OnSlingHeadDrag(Vector3 mousePosition)
    {
        Position = mousePosition;
    }

    public void OnSlingHeadDragEnd()
    {
        
    }
    
    public void AddForce(Vector3 force)
    {
        body.AddForce(force);
    }
    
    public void AddForce(Vector3 force, ForceMode forceMode)
    {
        body.AddForce(force, forceMode);
    }

    
    private void ApplyGravity()
    {
        if (!useGravity) return;
        
        body.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }
}
