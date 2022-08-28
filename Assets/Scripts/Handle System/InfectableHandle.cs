using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Handle_System
{
    public class InfectableHandle : Handle
    {
        [Header("Event Channels")]
        [SerializeField] private VoidEventChannel goalAchieved;
        
        [Header("References")]
        [SerializeField] private new MeshRenderer renderer;
        [SerializeField] private Material slimeMaterial;


        public bool IsInfected { get; private set; }


        protected override void OnAttached()
        {
            if (IsInfected) return;
            
            IsInfected = true;
            
            base.OnAttached();
            
            renderer.material = slimeMaterial;
            
            goalAchieved.RaiseEvent();
        }
    }
}