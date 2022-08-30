using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Handle_System
{
    public abstract class Handle : MonoBehaviour
    {
        public Vector3 Position => transform.position;


        public UnityEvent onAttached;

        
        private bool m_IsAttachable = true;
        private bool m_IsIgnored;


        public bool TryAttach()
        {
            if (m_IsIgnored) return false;
            
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

        public void IgnoreFor(float time)
        {
            m_IsIgnored = true;

            StartCoroutine(IgnoreRoutine());
            
            IEnumerator IgnoreRoutine()
            {
                yield return new WaitForSeconds(time);

                m_IsIgnored = false;
            }
        }
    }
}
