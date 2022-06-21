using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlingArm : MonoBehaviour
{
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

    public Vector3 LocalScale
    {
        get => transform.localScale;
        set => transform.localScale = value;
    }


    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }
}