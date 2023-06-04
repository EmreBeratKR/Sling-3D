using CutsceneSystem;
using ScriptableEvents.Core.Channels;
using UnityEngine;

public class ButtonCallbacks : MonoBehaviour
{
    [Header("Event Channels")]
    [SerializeField] private VoidEventChannel loadMainMenu;
    [SerializeField] private VoidEventChannel loadLevelMap;
    [SerializeField] private VoidEventChannel loadGame;


    public void OnClickPlayButton()
    {
        if (TutorialManager.IsTutorialCompleted(0))
        {
            RaiseLoadLevelMap();
            return;
        }
        
        SceneController.LoadCutScene(0);
    }
    
    
    public void RaiseLoadMainMenu()
    {
        loadMainMenu.RaiseEvent();
    }

    public void RaiseLoadLevelMap()
    {
        loadLevelMap.RaiseEvent();
    }
    
    public void RaiseLoadGame()
    {
        loadGame.RaiseEvent();
    }
}
