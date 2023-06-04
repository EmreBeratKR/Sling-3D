using UnityEngine;

namespace CutsceneSystem
{
    public class TutorialManager : MonoBehaviour
    {
        private Tutorial[] m_Tutorials;


        private int m_TutorialIndex = -1;
        

        private void Awake()
        {
            m_Tutorials = GetComponentsInChildren<Tutorial>();
        }


        public void NextTutorial()
        {
            m_TutorialIndex += 1;
            var tutorial = GetTutorial(m_TutorialIndex);
            tutorial.Begin();
        }


        private Tutorial GetTutorial(int index)
        {
            return m_Tutorials[index];
        }


        public static void MarkTutorialCompleted(int index)
        {
            PlayerPrefs.SetInt($"Tutorial_{index}", 1);
        }
        
        public static bool IsTutorialCompleted(int index)
        {
            return PlayerPrefs.GetInt($"Tutorial_{index}", 0) == 1;
        }
    }
}