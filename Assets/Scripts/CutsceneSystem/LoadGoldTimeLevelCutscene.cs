using UnityEngine;

namespace CutsceneSystem
{
    public class LoadGoldTimeLevelCutscene : MonoBehaviour
    {
        private const int NormalTimeCutsceneIndex = 10;
        private const int GoldTimeCutsceneIndex = 11;
        private const int AfterGoldTimeCutsceneIndex = 12;


        [SerializeField] private bool isLastLevel;
        

        public void Load()
        {
            if (TutorialManager.IsTutorialCompleted(GoldTimeCutsceneIndex))
            {
                SceneController.LoadCutScene(AfterGoldTimeCutsceneIndex);
                return;
            }
            
            SceneController.LoadCutScene(GetCutsceneIndex());
        }

        public bool IsLastLevel()
        {
            return isLastLevel;
        }


        private static int GetCutsceneIndex()
        {
            return LevelSaveSystem.IsAllLevelsGoldTime()
                ? GoldTimeCutsceneIndex
                : NormalTimeCutsceneIndex;
        }
    }
}