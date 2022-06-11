using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SlingMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlingBody slingBody;
    [SerializeField] private SlingArm slingArm;
    [SerializeField] private SlingRange slingRange;
    
    [Header("Speed Limitation")]
    [SerializeField] private float terminalSpeed;
    [SerializeField] private bool limitSpeed;
    
    [Header("Gravity")]
    [SerializeField] private float gravityScale;
    [SerializeField] private bool useGravity;

    private Rigidbody body;
    private bool isDragging;

    private SpringJointData springJointData;


    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;

        var springJoint = GetComponent<SpringJoint>();
        springJointData = new SpringJointData
        {
            connectedBody = springJoint.connectedBody,
            connectedAnchor = springJoint.connectedAnchor,
            spring = springJoint.spring,
            damper = springJoint.damper,
            minDistance = springJoint.minDistance,
            maxDistance = springJoint.maxDistance,
            tolerance = springJoint.tolerance
        };
    }

    private void OnMouseDown()
    {
        isDragging = true;

        if (TryGetComponent(out FixedJoint fixedJoint))
        {
            Destroy(fixedJoint);
            var newSpringJoint = gameObject.AddComponent<SpringJoint>();
            newSpringJoint.connectedBody = springJointData.connectedBody;
            newSpringJoint.autoConfigureConnectedAnchor = false;
            newSpringJoint.connectedAnchor = springJointData.connectedAnchor;
            newSpringJoint.spring = springJointData.spring;
            newSpringJoint.damper = springJointData.damper;
            newSpringJoint.minDistance = springJointData.minDistance;
            newSpringJoint.maxDistance = springJointData.maxDistance;
            newSpringJoint.tolerance = springJointData.tolerance;
        }
        
        slingArm.Attach();
        DisableGravity();
    }

    private void OnMouseDrag()
    {
        var mousePosition = MouseRaycaster.GetWorldPosition();
        
        if (!mousePosition.HasValue) return;

        var positionValue = mousePosition.Value;
        positionValue.z = transform.position.z;

        var armPosition = slingArm.transform.position;
        var distance = Vector3.Distance(positionValue, armPosition);
        var directionArmToHead = (positionValue - armPosition).normalized;

        directionArmToHead *= distance <= slingRange.Range ? distance : slingRange.Range;
        transform.position = armPosition + directionArmToHead;
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        LimitSpeed();
    }

    private void Update()
    {
        SlingShot();
    }

    public void EnableGravity()
    {
        useGravity = true;
    }

    public void DisableGravity()
    {
        useGravity = false;
    }

    private void ApplyGravity()
    {
        if (!useGravity) return;
        
        body.AddForce(Physics.gravity * gravityScale, ForceMode.Acceleration);
    }

    private void LimitSpeed()
    {
        if (!limitSpeed) return;
        
        var bodySpeed = body.velocity.magnitude;

        if (bodySpeed <= terminalSpeed) return;
        
        var speedExceed = terminalSpeed - bodySpeed;
        body.AddForce(body.velocity.normalized * speedExceed, ForceMode.VelocityChange);
    }

    private void SlingShot()
    {
        if (isDragging) return;
        
        if (slingBody.ArmLength > 1f) return;


        if (!TryGetComponent(out SpringJoint springJoint)) return;

        Destroy(springJoint);
        var joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = springJointData.connectedBody;
        
        slingArm.Detach();
        EnableGravity();
    }
}

public struct SpringJointData
{
    public Rigidbody connectedBody;
    public Vector3 connectedAnchor;
    public float spring;
    public float damper;
    public float minDistance;
    public float maxDistance;
    public float tolerance;
}