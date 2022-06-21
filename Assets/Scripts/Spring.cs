using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlingArm arm;
    [SerializeField] private SlingHead head;
    
    [Header("Values")]
    [SerializeField] private Vector3 equilibriumOffset;
    [SerializeField, Min(0f)] private float spring;
    [SerializeField] private bool active;


    private Vector3 EquilibriumPoint => arm.Position + equilibriumOffset;
    
    
    private void FixedUpdate()
    {
        if (!active) return;

        var force = (EquilibriumPoint - head.Position) * spring;
        
        head.AddForce(force);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(arm.Position, 0.1f);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(head.Position, 0.1f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(EquilibriumPoint, 0.1f);
    }
}
