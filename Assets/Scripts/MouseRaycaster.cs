using UnityEngine;

public class MouseRaycaster : MonoBehaviour
{
    [SerializeField] private LayerMask targetLayers;
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
            return info.point;
        }

        return null;
    }
}