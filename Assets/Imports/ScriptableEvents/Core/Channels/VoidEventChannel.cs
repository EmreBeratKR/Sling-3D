using UnityEngine;
using UnityEngine.Events;

namespace ScriptableEvents.Core.Channels
{
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannel : ScriptableObject
    {
        private UnityAction onEventRaised;


        public void RaiseEvent() => onEventRaised?.Invoke();

        public void AddListener(UnityAction action) => onEventRaised += action;
        public void RemoveListener(UnityAction action) => onEventRaised -= action;
    }
}
