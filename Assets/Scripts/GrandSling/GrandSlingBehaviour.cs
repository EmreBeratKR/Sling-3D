using System;
using Handle_System;
using UnityEngine;
using UnityEngine.Events;

namespace GrandSling
{
    public class GrandSlingBehaviour : MonoBehaviour
    {
        private static readonly int IsTalkingHash = Animator.StringToHash("isTalking");
        private static readonly int IsEyesOpenHash = Animator.StringToHash("isEyesOpen");
        
        
        [SerializeField] private Animator animator;


        public UnityAction<EventResponse> OnAttachedToHandle;
        public UnityAction<EventResponse> OnDetachedFromHandle;


        public void SetIsTalking(bool value)
        {
            animator.SetBool(IsTalkingHash, value);
        }

        public void SetIsEyesOpen(bool value)
        {
            animator.SetBool(IsEyesOpenHash, value);
        }


        [Serializable]
        public struct EventResponse
        {
            public Handle attachedHandle;
            public Handle detachedHandle;
        }
    }
}