using ScriptableEvents.Core.Channels;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableEvents.Core.Listeners
{
    public class VoidEventListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannel channel;

        public UnityEvent onEventRaised;


        private void OnEnable() => channel.AddListener(Respond);
        private void OnDisable() => channel.RemoveListener(Respond);

        private void Respond() => onEventRaised?.Invoke();
    }
}