using UnityEngine;

namespace UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject main;
        
        
        public void Open()
        {
            main.SetActive(true);
            PauseGame();
        }

        public void Close()
        {
            main.SetActive(false);
            ResumeGame();
        }

        
        public static void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public static void ResumeGame()
        {
            Time.timeScale = 1f;
        }
    }
}