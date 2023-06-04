using System;
using Sling;
using UnityEngine;
using UnityEngine.Events;

namespace CutsceneSystem
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] private SlingRange slingRange;
        
        
        public static UnityAction<CutsceneEventResponse> OnCutsceneInitialized;


        private void Start()
        {
            InitializeCutscene();
        }


        public void SetActiveSlingInput(bool value)
        {
            slingRange.InputEnabled = value;
        }

        public void LoadLevelMap()
        {
            SceneController.Instance.LoadLevelMap();
        }
        

        private void InitializeCutscene()
        {
            var response = new CutsceneEventResponse()
            {

            };
            
            OnCutsceneInitialized?.Invoke(response);
        }
    }


    [Serializable]
    public struct CutsceneEventResponse
    {
        
    }
}