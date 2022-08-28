using UnityEngine;
using UnityEngine.Events;

namespace Handle_System
{
    public abstract class Handle : MonoBehaviour
    {
        public Vector3 Position => transform.position;


        public UnityEvent onAttached;

        
        private bool m_IsAttachable = true;


        public bool TryAttach()
        {
            if (!m_IsAttachable) return false;
            
            OnAttached();

            return true;
        }

        protected virtual void OnAttached()
        {
            onAttached?.Invoke();
        }

        public virtual void MoveTo(Vector3 position)
        {
            transform.position = position;
        }

        public virtual void EnableHandle()
        {
            m_IsAttachable = true;
        }

        public virtual void DisableHandle()
        {
            m_IsAttachable = false;
        }
    }
}
