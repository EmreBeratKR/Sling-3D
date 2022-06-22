using UnityEngine;

public class SlingShape : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlingArm arm;
    [SerializeField] private SlingHead head;
    

    public float ArmLength => Vector3.Distance(head.Position, arm.Position);
    public float SqrArmLength => Vector3.SqrMagnitude(head.Position - arm.Position);
    
        
    private void Update()
    {
        UpdateShape();
    }
    
    
    private void UpdateShape()
    {
        var headPosition = head.Position;
        var armPosition = arm.Position;

        var deltaX = headPosition.x - armPosition.x;
        var deltaY = armPosition.y - headPosition.y;

        var angle = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;
        var eulerAngle = Vector3.forward * angle;

        head.EulerAngles = eulerAngle;
        arm.EulerAngles = eulerAngle;
        
        arm.LocalScale = new Vector3(1f, ArmLength, 1f);
    }
}
