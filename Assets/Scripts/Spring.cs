using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SlingArm arm;
    [SerializeField] private SlingHead head;
    
    [Header("Values")]
    [SerializeField] private Vector3 equilibriumOffset;
    [SerializeField, Min(0f)] private float spring;
    [SerializeField] private SpringMode mode;
    

    private Vector3 EquilibriumPoint
    {
        get
        {
            if (mode == SpringMode.EnabledNoOffset)
            {
                return arm.Position;
            }
            
            return arm.Position + equilibriumOffset;
        }
    }
    
    
    private void FixedUpdate()
    {
        if (mode == SpringMode.Disabled) return;

        var force = (EquilibriumPoint - head.Position) * spring;
        
        head.AddForce(force);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(EquilibriumPoint, 0.1f);
    }
    
    
    
    
    public enum SpringMode
    {
        Disabled,
        Enabled,
        EnabledNoOffset
    }
}
