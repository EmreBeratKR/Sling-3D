using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CustomPlayables
{
    public class TransformClip : PlayableAsset, ITimelineClipAsset
    {
        public Data from = Data.Default;
        public Data to = Data.Default;
        
        public AnimationCurveGroup positionCurve = AnimationCurveGroup.Default;
        public AnimationCurveGroup rotationCurve = AnimationCurveGroup.Default;
        public AnimationCurveGroup scaleCurve = AnimationCurveGroup.Default;
        
        public Vector3Constraints positionConstraints = (Vector3Constraints) ~0;
        public Vector3Constraints rotationConstraints = (Vector3Constraints) ~0;
        public Vector3Constraints scaleConstraints = (Vector3Constraints) ~0;
        
        
        public ClipCaps clipCaps => ClipCaps.Blending;


        private TransformClipBehaviour m_Behaviour;
        
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TransformClipBehaviour>.Create(graph);
            m_Behaviour = playable.GetBehaviour();

            m_Behaviour.from = from;
            m_Behaviour.to = to;
            
            m_Behaviour.positionCurve = positionCurve;
            m_Behaviour.rotationCurve = rotationCurve;
            m_Behaviour.scaleCurve = scaleCurve;

            m_Behaviour.positionConstraints = positionConstraints;
            m_Behaviour.rotationConstraints = rotationConstraints;
            m_Behaviour.scaleConstraints = scaleConstraints;
            
            return playable;
        }


        public void RestoreDefaultValues()
        {
            m_Behaviour.RestoreDefaultValues();
        }
        
        
        [Serializable]
        public struct Data
        {
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;


            public static Data Default => new Data()
            {
                scale = Vector3.one
            };
            

            public Quaternion Rotation => Quaternion.Euler(rotation);
        }
        
        
        [Serializable]
        public struct AnimationCurveGroup
        {
            public AnimationCurve x, y, z;


            public static AnimationCurveGroup Default => new AnimationCurveGroup()
            {
                x = AnimationCurve.Linear(0f, 0f, 1f, 1f),
                y = AnimationCurve.Linear(0f, 0f, 1f, 1f),
                z = AnimationCurve.Linear(0f, 0f, 1f, 1f)
            };
        }
        
        
        [Flags]
        public enum Vector3Constraints
        {
            X = 1 << 1,
            Y = 1 << 2,
            Z = 1 << 3
        }
    }
}