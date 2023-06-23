using System;
using UnityEditor;
using UnityEngine;

namespace DeveloperTools.Editor
{
    public partial class DeveloperPanel : EditorWindow
    {
        private const string Title = "Developer Panel";


        private static readonly string[] Tabs =
        {
            "Level"
        };


        private PageTab m_CurrentPageTab;


        private static void Button(string label, Action onClicked)
        {
            if (GUILayout.Button(label))
            {
                onClicked?.Invoke();
            }
        }
        
        [MenuItem("Developer/" + Title)]
        private static void ShowWindow() 
        { 
            GetWindow<DeveloperPanel>(Title);
        }
    
        private void OnGUI() 
        {
            OnPageTabGUI();
            OnCurrentTabGUI();
        }


        private void OnPageTabGUI()
        {
            m_CurrentPageTab = (PageTab) GUILayout.Toolbar((int) m_CurrentPageTab, Tabs);
            GUILayout.Space(10);
        }

        private void OnCurrentTabGUI()
        {
            switch (m_CurrentPageTab)
            {
                case PageTab.Level:
                    OnLevelGUI();
                    break;
            }
        }


        private enum PageTab
        {
            Level
        }
    }
}