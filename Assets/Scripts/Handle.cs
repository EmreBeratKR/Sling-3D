using UnityEngine;

public class Handle : MonoBehaviour
{
    [SerializeField] private new MeshRenderer renderer;
    [SerializeField] private Material slimeMaterial;


    public Vector3 Position => transform.position;
    
    
    public void InfectSlime()
    {
        renderer.material = slimeMaterial;
    }
}
