using NaughtyAttributes;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D normal;
    [SerializeField] private Texture2D stretch;


    private void Start()
    {
        SetNormal();
    }


    public void OnEnteredStretchRange()
    {
        SetStretch();
    }
    
    public void OnExitedStretchRange()
    {
        SetNormal();
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void SetNormal()
    {
        var hotspot = Vector2.zero;
        Cursor.SetCursor(normal, hotspot, CursorMode.ForceSoftware);
    }
    
    [Button(enabledMode: EButtonEnableMode.Playmode)]
    private void SetStretch()
    {
        var hotspot = new Vector2(stretch.height * 0.5f, stretch.width * 0.5f);
        Cursor.SetCursor(stretch, hotspot, CursorMode.ForceSoftware);
    }
}
