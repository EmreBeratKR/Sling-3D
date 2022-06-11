using UnityEngine;

public class SlingBody : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private Transform arm;


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

        var armLength = Vector3.Distance(headPosition, armPosition);
        arm.localScale = new Vector3(1f, armLength, 1f);
    }
}
