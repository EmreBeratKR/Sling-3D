using UnityEngine;

namespace Handle_System
{
    public class DirtyInfectableHandle : InfectableHandle, ICleanable
    {
        [SerializeField] private GameObject dirtyVisual;


        public bool IsDirty
        {
            get => dirtyVisual.activeSelf;
            private set => dirtyVisual.SetActive(value);
        }
        
        
        protected override void OnAttached()
        {
            if (IsDirty) return;
            
            base.OnAttached();
        }

        public void Clean()
        {
            IsDirty = false;
            OnAttached();
        }
    }
}