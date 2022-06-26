using System.Collections;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Handle_System
{
    public class PortalHandle : Handle
    {
        [SerializeField] private VoidEventChannel slingAttachedToPortal;
        [SerializeField] private VoidEventChannel portalEnteringStart;
    
    
        public override void OnAttached()
        {
            slingAttachedToPortal.RaiseEvent();

            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(1f);
                portalEnteringStart.RaiseEvent();
            }
        }
    }
}