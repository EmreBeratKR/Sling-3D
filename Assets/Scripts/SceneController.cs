using System.Collections;
using Helpers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    [SerializeField, Scene] private int 
        mainMenu,
        levelMap,
        game;
    
    
    public void OnLevelFailed()
    {
        RestartLevel();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void LoadLevelMap()
    {
        SceneManager.LoadScene(levelMap);
    }
    
    public void LoadGame()
    {
        SceneManager.LoadScene(game);
    }
    
    
    private void RestartLevel()
    {
        StartCoroutine(Restart());
        
        IEnumerator Restart()
        {
            yield return new WaitForSeconds(1f);
            LoadGame();
        }
    }
}
