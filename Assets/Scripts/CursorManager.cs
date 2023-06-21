using System;
using Helpers;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    [SerializeField] private RectTransform cursorMain;
    [SerializeField] private GameObject normalCursor;
    [SerializeField] private GameObject hoverCursor;
    [SerializeField] private GameObject grabbedCursor;


    private static bool m_Initialized;
    

    protected override void Awake()
    {
        base.Awake();
        
        if (m_Initialized) return;
        
        HideSystemCursor();
        SetNormal();

        m_Initialized = true;
    }

    private void Update()
    {
        cursorMain.position = Input.mousePosition;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = !hasFocus;
    }


    public void SetNormal()
    {
        normalCursor.SetActive(true);
        hoverCursor.SetActive(false);
        grabbedCursor.SetActive(false);
    }

    public void SetHover()
    {
        normalCursor.SetActive(false);
        hoverCursor.SetActive(true);
        grabbedCursor.SetActive(false);
    }
    
    public void SetGrabbed()
    {
        normalCursor.SetActive(false);
        hoverCursor.SetActive(false);
        grabbedCursor.SetActive(true);
    }

    public void SetGrabbedEulerAngles(Vector3 eulerAngles)
    {
        grabbedCursor.transform.eulerAngles = eulerAngles;
    }
    
    
    private static void HideSystemCursor()
    {
        Cursor.visible = false;
    }
}
