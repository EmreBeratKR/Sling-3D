using Handle_System;
using Sling;
using UnityEngine;

namespace CutsceneSystem
{
    public class GrabHandleTutorial : Tutorial
    {
        [SerializeField] private SlingArm arm;
        [SerializeField] private Handle handle;


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
            if (!isBegin) return;
            
            if (isComplete) return;
            
            if (grabHandle != handle) return;

            Complete();
        }
    }
}