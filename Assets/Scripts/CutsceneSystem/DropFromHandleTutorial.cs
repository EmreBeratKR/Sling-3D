using Handle_System;
using Sling;
using UnityEngine;

namespace CutsceneSystem
{
    public class DropFromHandleTutorial : Tutorial
    {
        [SerializeField] private SlingArm arm;


        private void Awake()
        {
            arm.OnDetachHandle += Arm_OnDetachHandle;
        }

        private void OnDestroy()
        {
            arm.OnDetachHandle -= Arm_OnDetachHandle;
        }


        private void Arm_OnDetachHandle(Handle detachHandle)
        {
            if (!isBegin) return;
            
            if (isComplete) return;

            Complete();
        }
    }
}