using Handle_System;
using UnityEngine;

namespace CutsceneSystem
{
    public class EnterPortalTutorial : Tutorial
    {
        [SerializeField] private Portal portal;


        private PortalHandle m_PortalHandle;
        

        private void Awake()
        {
            m_PortalHandle = portal.GetHandle();
            
            m_PortalHandle.OnSlingAttached += PortalHandle_OnSlingAttached;
        }

        private void OnDestroy()
        {
            m_PortalHandle.OnSlingAttached -= PortalHandle_OnSlingAttached;
        }

        
        private void PortalHandle_OnSlingAttached()
        {
            if (!isBegin) return;
            
            if (isComplete) return;

            Complete();
        }
    }
}