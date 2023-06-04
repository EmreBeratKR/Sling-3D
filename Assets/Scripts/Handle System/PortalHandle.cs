using System;
using System.Collections;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Handle_System
{
    public class PortalHandle : Handle
    {
        [SerializeField] private VoidEventChannel slingAttachedToPortal;
        [SerializeField] private VoidEventChannel portalEnteringStart;
        [SerializeField] private PortalSound sound;


        public event Action OnSlingAttached;
        

        protected override void OnAttached()
        {
            base.OnAttached();
            
            slingAttachedToPortal.RaiseEvent();
            OnSlingAttached?.Invoke();

            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(1f);
                portalEnteringStart.RaiseEvent();
                sound.PlaySlingEntering();
            }
        }
    }
}