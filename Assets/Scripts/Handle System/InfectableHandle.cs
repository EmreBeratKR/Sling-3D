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


        public override void OnAttached()
        {
            if (IsInfected) return;
            
            IsInfected = true;
            
            renderer.material = slimeMaterial;
            
            goalAchieved.RaiseEvent();
        }
    }
}