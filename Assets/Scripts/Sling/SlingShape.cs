using UnityEngine;

namespace Sling
{
    public class SlingShape : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SlingArm arm;
        [SerializeField] private SlingHead head;
    

        public float ArmLength => Vector3.Distance(head.Position, arm.Position);
        public float SqrArmLength => Vector3.SqrMagnitude(head.Position - arm.Position);
    
        
        private void Update()
        {
            UpdateShape();
        }


        public void OnPortalEnteringStarted()
        {
            Destroy(this);
        }


        public Vector3 GetStretchEulerAngles()
        {
            var headPosition = head.Position;
            var armPosition = arm.Position;

            var deltaX = headPosition.x - armPosition.x;
            var deltaY = armPosition.y - headPosition.y;

            var angle = Mathf.Atan2(deltaX, deltaY) * Mathf.Rad2Deg;
            
            return Vector3.forward * angle;
        }

        public void UpdateArmLength(float armLength)
        {
            arm.Position = head.Position + head.transform.up * armLength;
            arm.LocalScale = new Vector3(1f, armLength, 1f);
        }
        
        
        private void UpdateShape()
        {
            var eulerAngle = GetStretchEulerAngles();
            
            head.EulerAngles = eulerAngle;
            arm.EulerAngles = eulerAngle;
        
            arm.LocalScale = new Vector3(1f, ArmLength, 1f);
        }
    }
}
