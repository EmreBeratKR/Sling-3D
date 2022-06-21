using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlingHead : MonoBehaviour
{
    [SerializeField] private float gravityScale;
    [SerializeField] private bool useGravity;
    private Rigidbody body;


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
    }

    private void FixedUpdate()
    {
        ApplyGravity();   
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
