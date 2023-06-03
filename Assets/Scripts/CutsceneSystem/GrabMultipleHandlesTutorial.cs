using System.Linq;
using Handle_System;
using Sling;
using UnityEngine;

namespace CutsceneSystem
{
    public class GrabMultipleHandlesTutorial : Tutorial
    {
        [SerializeField] private SlingArm arm;
        [SerializeField] private Handle[] handles;


        private bool[] m_CompleteState;


        private void Awake()
        {
            m_CompleteState = new bool[handles.Length];
            
            arm.OnGrabHandle += Arm_OnGrabHandle;
        }

        private void OnDestroy()
        {
            arm.OnGrabHandle -= Arm_OnGrabHandle;
        }

        
        private void Arm_OnGrabHandle(Handle handle)
        {
            if (!isBegin) return;
            
            if (isComplete) return;

            for (var i = 0; i < handles.Length; i++)
            {
                if (handles[i] != handle) continue;
                
                m_CompleteState[i] = true;

                if (IsCompleted())
                {
                    Complete();
                    Debug.Log("completed hurraaa");
                }
            }
        }

        private bool IsCompleted()
        {
            return m_CompleteState.All(value => value);
        }
    }
}