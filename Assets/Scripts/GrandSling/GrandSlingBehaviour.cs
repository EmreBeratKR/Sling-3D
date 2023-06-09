using System;
using Handle_System;
using UnityEngine;
using UnityEngine.Events;

namespace GrandSling
{
    public class GrandSlingBehaviour : MonoBehaviour
    {
        private static readonly int IsTalkingHash = Animator.StringToHash("isTalking");
        
        
        [SerializeField] private Animator animator;


        public UnityAction<EventResponse> OnAttachedToHandle;
        public UnityAction<EventResponse> OnDetachedFromHandle;


        public void SetIsTalking(bool value)
        {
            animator.SetBool(IsTalkingHash, value);
        }


        [Serializable]
        public struct EventResponse
        {
            public Handle attachedHandle;
            public Handle detachedHandle;
        }
    }
}