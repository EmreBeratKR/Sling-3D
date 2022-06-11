using UnityEngine;

public class SlingMovement : MonoBehaviour
{
    private void OnMouseDrag()
    {
        var mousePosition = MouseRaycaster.GetWorldPosition();
        
        if (!mousePosition.HasValue) return;

        var positionValue = mousePosition.Value;
        positionValue.z = transform.position.z;

        transform.position = positionValue;
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        var position = MouseRaycaster.GetWorldPosition();
        
        if (!position.HasValue) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(position.Value, 0.1f);
    }
}
