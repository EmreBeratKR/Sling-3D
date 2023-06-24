using UnityEngine;

namespace CutsceneSystem
{
    public class LoadLevelCutScene : MonoBehaviour
    {
        [SerializeField] private int cutSceneIndex;


        public void Load()
        {
            SceneController.LoadCutScene(cutSceneIndex);
        }

        public bool IsCompleted()
        {
            return TutorialManager.IsTutorialCompleted(cutSceneIndex);
        }
    }
}