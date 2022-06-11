using UnityEngine;

public class SlingBody : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform arm;


    public float ArmLength => Vector3.Distance(head.position, arm.position);
    
        
    private void Update()
    {
        UpdateShape();
    }
    
    private void UpdateShape()
    {
        var headPosition = head.position;
        var armPosition = arm.position;

        var deltaX = headPosition.x - armPosition.x;
        var deltaY = armPosition.y - headPosition.y;

        var angle = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;
        var eulerAngle = Vector3.forward * angle;

        head.eulerAngles = eulerAngle;
        arm.eulerAngles = eulerAngle;
        
        arm.localScale = new Vector3(1f, ArmLength, 1f);
    }
}
