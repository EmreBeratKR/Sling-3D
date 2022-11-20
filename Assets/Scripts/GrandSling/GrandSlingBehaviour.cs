using System;
using Handle_System;
using UnityEngine;
using UnityEngine.Events;

namespace GrandSling
{
    public class GrandSlingBehaviour : MonoBehaviour
    {
        public UnityAction<EventResponse> OnAttachedToHandle;
        public UnityAction<EventResponse> OnDetachedFromHandle;
        
        
        
        
        
        
        [Serializable]
        public struct EventResponse
        {
            public Handle attachedHandle;
            public Handle detachedHandle;
        }
    }
}