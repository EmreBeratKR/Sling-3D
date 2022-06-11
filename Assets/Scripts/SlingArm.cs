using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlingArm : MonoBehaviour
{
    private Rigidbody body;


    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Attach()
    {
        body.isKinematic = true;
    }

    public void Detach()
    {
        body.isKinematic = false;
    }
}
