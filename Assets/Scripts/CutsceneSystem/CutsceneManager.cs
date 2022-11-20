using System;
using UnityEngine;
using UnityEngine.Events;

namespace CutsceneSystem
{
    public class CutsceneManager : MonoBehaviour
    {
        public static UnityAction<CutsceneEventResponse> OnCutsceneInitialized;


        private void Start()
        {
            InitializeCutscene();
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