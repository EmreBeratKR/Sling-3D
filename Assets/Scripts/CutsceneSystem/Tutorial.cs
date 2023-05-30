using UnityEngine;
using UnityEngine.Events;

namespace CutsceneSystem
{
    public abstract class Tutorial : MonoBehaviour
    {
        public UnityEvent onBegin;
        public UnityEvent onComplete;


        protected bool isBegin;
        protected bool isComplete;
        
        
        public void Begin()
        {
            isBegin = true;
            onBegin?.Invoke();
        }

        
        protected void Complete()
        {
            isComplete = true;
            onComplete?.Invoke();
        }
    }
}