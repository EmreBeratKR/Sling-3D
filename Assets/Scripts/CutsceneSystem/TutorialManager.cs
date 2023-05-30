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
    }
}