using UnityEngine;
using UnityEngine.SceneManagement;

namespace CutsceneSystem
{
    public class LoadLevelCutScene : MonoBehaviour
    {
        [SerializeField] private int cutSceneIndex;


        public void Load()
        {
            SceneController.LoadCutScene(cutSceneIndex);
        }
    }
}