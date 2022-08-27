using UnityEngine;
using UnityEngine.Events;

namespace Handle_System
{
    public abstract class Handle : MonoBehaviour
    {
        public Vector3 Position => transform.position;


        public UnityEvent onAttached;


        private SphereCollider m_SphereCollider;

        private SphereCollider Collider
        {
            get
            {
                if (!m_SphereCollider)
                {
                    m_SphereCollider = GetComponent<SphereCollider>();
                }

                return m_SphereCollider;
            }
        }


        public virtual void OnAttached()
        {
            onAttached?.Invoke();
        }

        public virtual void MoveTo(Vector3 position)
        {
            transform.position = position;
        }

        public virtual void Enable()
        {
            Collider.enabled = true;
        }

        public virtual void Disable()
        {
            Collider.enabled = false;
        }
    }
}
