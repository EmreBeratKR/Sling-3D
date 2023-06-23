using Goal_System;
using Sling;
using UnityEditor;
using UnityEngine;

namespace DeveloperTools.Editor
{
    public partial class DeveloperPanel
    {
        private int m_Level = 1;
        
        
        private void OnLevelGUI()
        {
            m_Level = Mathf.Clamp(EditorGUILayout.IntField("Level", m_Level), 1, LevelSystem.LevelCount);
            
            Button($"Unlock Level {m_Level}", () =>
            {
                var levelSave = new LevelSave
                {
                    state = LevelState.NotPlayed
                };
                
                LevelSaveSystem.Save(levelSave, m_Level - 1);
            });
            
            Button("Unlock All Levels", () =>
            {
                for (var i = 0; i < LevelSystem.LevelCount; i++)
                {
                    var levelSave = new LevelSave
                    {
                        state = LevelState.NotPlayed
                    };
                
                    LevelSaveSystem.Save(levelSave, i);
                }
            });
            
            Button("Complete Current Level", () =>
            {
                var slingBehaviour = FindObjectOfType<SlingBehaviour>();

                if (slingBehaviour)
                {
                    slingBehaviour.Developer_Panel_CompleteLevel();
                }
            });
        }
    }
}