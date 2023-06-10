using UnityEngine;

public class SyncMaterialTilingWithScale : MonoBehaviour
{
    [SerializeField] private new Renderer renderer;
    [SerializeField] private Vector2 tiling = Vector2.one;


    private void Awake()
    {
        Sync();
    }


    private void Sync()
    {
        var material = renderer.material;
        var syncTiling = GetSyncTiling();

        material.mainTextureScale = syncTiling;
    }

    private Vector2 GetSyncTiling()
    {
        var scale = GetScale();
        var x = tiling.x * scale.x;
        var y = tiling.y * scale.y;

        return new Vector2(x, y);
    }

    private Vector3 GetScale()
    {
        return transform.lossyScale;
    }
}