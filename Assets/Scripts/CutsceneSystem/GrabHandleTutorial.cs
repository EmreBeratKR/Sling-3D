using Handle_System;
using Sling;
using UnityEngine;
using UnityEngine.Events;

namespace CutsceneSystem
{
    public class GrabHandleTutorial : MonoBehaviour
    {
        [SerializeField] private SlingArm arm;
        [SerializeField] private Handle handle;
        
        
        public UnityEvent onBegin;
        public UnityEvent onComplete;


        private bool m_IsBegin;
        private bool m_IsComplete;
        

        private void Awake()
        {
            arm.OnGrabHandle += Arm_OnGrabHandle;
        }

        private void OnDestroy()
        {
            arm.OnGrabHandle -= Arm_OnGrabHandle;
        }


        private void Arm_OnGrabHandle(Handle grabHandle)
        {
            if (!m_IsBegin) return;
            
            if (m_IsComplete) return;
            
            if (grabHandle != handle) return;

            m_IsComplete = true;
            onComplete?.Invoke();
        }


        public void Begin()
        {
            m_IsBegin = true;
            onBegin?.Invoke();
        }
    }
}