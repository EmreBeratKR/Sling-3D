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

        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TransformClipBehaviour>.Create(graph);
            var playableBehaviour = playable.GetBehaviour();

            playableBehaviour.from = from;
            playableBehaviour.to = to;
            
            playableBehaviour.positionCurve = positionCurve;
            playableBehaviour.rotationCurve = rotationCurve;
            playableBehaviour.scaleCurve = scaleCurve;

            playableBehaviour.positionConstraints = positionConstraints;
            playableBehaviour.rotationConstraints = rotationConstraints;
            playableBehaviour.scaleConstraints = scaleConstraints;
            
            return playable;
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