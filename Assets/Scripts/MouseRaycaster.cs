using UnityEngine;

public class MouseRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private bool debug;
    
    
    private static LayerMask TargetLayers => Instance.targetLayers;
    
    
    private static MouseRaycaster instance;
    private static MouseRaycaster Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MouseRaycaster>();
            }

            return instance;
        }
    }
    
    
    private static Camera mainCamera;
    private static Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            return mainCamera;
        }
    }

    public static Vector3? GetWorldPosition()
    {
        var ray = MainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var info, float.PositiveInfinity, TargetLayers))
        {
            var point = info.point;
            point.z = 0f;
            return point;
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        if (!debug) return;
        
        var mousePos = GetWorldPosition();
        
        if (!mousePos.HasValue) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(mousePos.Value, 0.1f);
    }
}