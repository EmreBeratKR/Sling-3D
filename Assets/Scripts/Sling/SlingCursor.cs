using UnityEngine;

namespace Sling
{
    public class SlingCursor : MonoBehaviour
    {
        [SerializeField] private SlingRange range;
        [SerializeField] private SlingShape shape;


        private State m_State;
        
        
        private void OnMouseOver()
        {
            if (range.InputEnabled)
            {
                m_State = State.Hover;
                
                if (range.IsDragging) return;

                CursorManager.Instance.SetHover();
                return;
            }
            
            m_State = State.Normal;

            if (range.IsDragging) return;
            
            CursorManager.Instance.SetNormal();
        }

        private void OnMouseExit()
        {
            m_State = State.Normal;
            
            if (range.IsDragging) return;
            
            CursorManager.Instance.SetNormal();
        }

        private void Update()
        {
            if (shape)
            {
                CursorManager.Instance.SetGrabbedEulerAngles(shape.GetStretchEulerAngles());
            }

            if (range.IsDragging)
            {
                CursorManager.Instance.SetGrabbed();
                return;
            }

            switch (m_State)
            {
                case State.Normal:
                    CursorManager.Instance.SetNormal();
                    break;
                
                case State.Hover:
                    CursorManager.Instance.SetHover();
                    break;
            }
        }


        public void OnSlingDied()
        {
            m_State = State.Normal;
            CursorManager.Instance.SetNormal();
        }
        
        
        private enum State
        {
            Normal,
            Hover,
            Grabbed
        }
    }
}