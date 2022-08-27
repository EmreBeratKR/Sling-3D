using UnityEngine;
using UnityEngine.Events;

namespace Handle_System
{
    public abstract class Handle : MonoBehaviour
    {
        public Vector3 Position => transform.position;


        public UnityEvent onAttached;


        public virtual void OnAttached()
        {
            onAttached?.Invoke();
        }

        public virtual void MoveTo(Vector3 position)
        {
            transform.position = position;
        }
    }
}
