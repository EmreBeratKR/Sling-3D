using System.Collections;
using Helpers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    private const int BonusLevelIndex = 3;
    private const int CutSceneStartIndex = 4;
    
    
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
            SceneTransition.FadeInOutSlime();
        }
    }
    
    
    public static void LoadBonusLevel()
    {
        SceneManager.LoadScene(BonusLevelIndex);
    }
    
    public static void LoadCutScene(int index)
    {
        Instance.StartCoroutine(Routine());
        
        IEnumerator Routine()
        {
            SceneManager.LoadScene(CutSceneStartIndex + index);
            yield return null;
            SceneTransition.FadeInOutSlime();
        }
    }
}
