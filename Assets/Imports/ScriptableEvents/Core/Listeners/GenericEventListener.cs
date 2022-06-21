using ScriptableEvents.Core.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableEvents.Core.Listeners
{
    public abstract class GenericEventListener<T> : MonoBehaviour
    {
        [SerializeField] private GenericEventChannel<T> channel;

        public UnityEvent<T> onEventRaised;


        protected virtual void OnEnable() => channel.AddListener(Respond);
        protected virtual void OnDisable() => channel.RemoveListener(Respond);

        private void Respond(T data) => onEventRaised?.Invoke(data);
    }
}