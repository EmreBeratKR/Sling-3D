using UnityEngine;
using UnityEngine.Events;

namespace ScriptableEvents.Core.Channels
{
    public abstract class GenericEventChannel<T> : ScriptableObject
    {
        private UnityAction<T> onEventRaised;


        public void RaiseEvent(T data) => onEventRaised?.Invoke(data);

        public void AddListener(UnityAction<T> action) => onEventRaised += action;
        public void RemoveListener(UnityAction<T> action) => onEventRaised -= action;
    }
}