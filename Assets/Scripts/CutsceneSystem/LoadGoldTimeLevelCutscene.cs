using UnityEngine;

namespace CutsceneSystem
{
    public class LoadGoldTimeLevelCutscene : MonoBehaviour
    {
        private const int NormalTimeCutsceneIndex = 10;
        private const int GoldTimeCutsceneIndex = 11;


        public void Load()
        {
            SceneController.LoadCutScene(GetCutsceneIndex());
        }


        private static int GetCutsceneIndex()
        {
            return LevelSaveSystem.IsAllLevelsGoldTime()
                ? GoldTimeCutsceneIndex
                : NormalTimeCutsceneIndex;
        }
    }
}