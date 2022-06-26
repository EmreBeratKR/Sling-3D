using UnityEngine;

namespace Handle_System
{
    public class Handle : MonoBehaviour
    {
        public Vector3 Position => transform.position;


        public virtual void OnAttached()
        {
        
        }
    }
}
