using UnityEngine;

namespace GrandSling
{
    public class GrandSlingSpring : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private GrandSlingBehaviour behaviour;
        [SerializeField] private GrandSlingArm arm;
        [SerializeField] private GrandSlingHead head;

        [Header("Values")]
        [SerializeField] private Vector3 equilibriumOffset;
        [SerializeField, Min(0f)] private float spring;


        private Vector3 EquilibriumPoint => arm.Position + equilibriumOffset;


        private bool m_IsActive;


        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void FixedUpdate()
        {
            ApplySpringForce();
        }


        private void OnGrandSlingAttachedToHandle(GrandSlingBehaviour.EventResponse response)
        {
            m_IsActive = true;
        }

        private void OnGrandSlingDetachedFromHandle(GrandSlingBehaviour.EventResponse response)
        {
            m_IsActive = false;
        }
        

        private void ApplySpringForce()
        {
            if (!m_IsActive) return;

            var springForce = (EquilibriumPoint - head.Position) * spring;
            
            head.AddForce(springForce);
        }

        private void AddListeners()
        {
            if (behaviour)
            {
                behaviour.OnAttachedToHandle += OnGrandSlingAttachedToHandle;
                behaviour.OnDetachedFromHandle += OnGrandSlingDetachedFromHandle;
            }
        }

        private void RemoveListeners()
        {
            if (behaviour)
            {
                behaviour.OnAttachedToHandle -= OnGrandSlingAttachedToHandle;
                behaviour.OnDetachedFromHandle -= OnGrandSlingDetachedFromHandle;
            }
        }
    }
}