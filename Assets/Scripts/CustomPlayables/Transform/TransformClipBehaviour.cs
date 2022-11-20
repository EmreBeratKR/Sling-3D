using System;
using CustomPlayables.Extensions;
using UnityEngine;
using UnityEngine.Playables;

namespace CustomPlayables
{
    [Serializable]
    public class TransformClipBehaviour : PlayableBehaviour
    {
        public TransformClip.Data from;
        public TransformClip.Data to;
        
        public TransformClip.AnimationCurveGroup positionCurve;
        public TransformClip.AnimationCurveGroup rotationCurve;
        public TransformClip.AnimationCurveGroup scaleCurve;
        
        public TransformClip.Vector3Constraints positionConstraints;
        public TransformClip.Vector3Constraints rotationConstraints;
        public TransformClip.Vector3Constraints scaleConstraints;


        private Transform m_BindingTransform;
        private Vector3 m_DefaultPosition;
        private Quaternion m_DefaultRotation;
        private Vector3 m_DefaultScale;
        private bool m_IsFirstFrame = true;
        
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            m_BindingTransform = playerData as Transform;
            
            if (!m_BindingTransform) return;

            
            if (m_IsFirstFrame)
            {
                m_DefaultPosition = m_BindingTransform.position;
                m_DefaultRotation = m_BindingTransform.rotation;
                m_DefaultScale = m_BindingTransform.localScale;
                
                m_IsFirstFrame = false;
            }
            
            var normalizedTime = playable.GetNormalizedTime();
            
            var position = ProcessVector3(m_BindingTransform.position, from.position, to.position, positionConstraints, positionCurve, normalizedTime);
            m_BindingTransform.position = position;

            var rotation = ProcessQuaternion(m_BindingTransform.rotation, from.rotation, to.rotation, rotationConstraints, rotationCurve, normalizedTime);
            m_BindingTransform.rotation = rotation;
            
            var scale = ProcessVector3(m_BindingTransform.localScale, from.scale, to.scale, scaleConstraints, scaleCurve, normalizedTime);
            m_BindingTransform.localScale = scale;
        }
        

        private static Vector3 ProcessVector3(
            Vector3 current, 
            Vector3 from, 
            Vector3 to, 
            TransformClip.Vector3Constraints constraints, 
            TransformClip.AnimationCurveGroup curveGroup, 
            float t)
        {
            var result = current;

            if (constraints.ContainsX())
            {
                result.x = Mathf.LerpUnclamped(from.x, to.x, curveGroup.x.Evaluate(t));
            }
            
            if (constraints.ContainsY())
            {
                result.y = Mathf.LerpUnclamped(from.y, to.y, curveGroup.y.Evaluate(t));
            }
            
            if (constraints.ContainsZ())
            {
                result.z = Mathf.LerpUnclamped(from.z, to.z, curveGroup.z.Evaluate(t));
            }

            return result;
        }

        private static Quaternion ProcessQuaternion(
            Quaternion current, 
            Vector3 from, 
            Vector3 to, 
            TransformClip.Vector3Constraints constraints, 
            TransformClip.AnimationCurveGroup curveGroup,
            float t)
        {
            var currentEulerAngles = current.eulerAngles;
            var constrainedFrom = ProcessVector3(currentEulerAngles, from, from, constraints, curveGroup, t);
            var constrainedTo = ProcessVector3(currentEulerAngles, to, to, constraints, curveGroup, t);

            return Quaternion.LerpUnclamped(Quaternion.Euler(constrainedFrom), Quaternion.Euler(constrainedTo), t);
        }

        public void RestoreDefaultValues()
        {
            if (!m_BindingTransform) return;

            m_BindingTransform.position = m_DefaultPosition;
            m_BindingTransform.rotation = m_DefaultRotation;
            m_BindingTransform.localScale = m_DefaultScale;
        }
    }
}