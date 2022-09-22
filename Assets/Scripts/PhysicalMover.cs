using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicalMover : MonoBehaviour
{
    [SerializeField] private Motion[] motions;
    [SerializeField] private bool playOnStart;
    [SerializeField] private bool isLooping;


    private Rigidbody m_Body;


    private void Start()
    {
        m_Body = GetComponent<Rigidbody>();

        if (playOnStart)
        {
            PlayMotions();
        }
    }


    public void PlayMotions()
    {
        PlayMotion(motions[0], 0);
    }
    

    private void PlayMotion(Motion motion, int motionIndex)
    {
        var tween = m_Body.DOMove(motion.to, motion.duration)
            .From(motion.from)
            .SetEase(motion.ease)
            .SetDelay(motion.delay)
            .SetSpeedBased(motion.isSpeedBased);

        var isLastMotion = motionIndex == motions.Length - 1;
        
        if (isLastMotion && !isLooping) return;
        
        tween.OnComplete(() =>
        {
            PlayMotion(GetNextMotionByIndex(motionIndex), GetNextMotionIndex(motionIndex));
        });
    }
    
    private int GetNextMotionIndex(int motionIndex)
    {
        var nextMotionIndex = motionIndex + 1;
        var isLastIndex = motionIndex == motions.Length - 1;
        return isLastIndex ? 0 : nextMotionIndex;
    }
    
    private Motion GetNextMotionByIndex(int motionIndex)
    {
        return motions[GetNextMotionIndex(motionIndex)];
    }


    [Serializable]
    private struct Motion
    {
        public Vector3 from;
        public Vector3 to;
        public Ease ease;
        public float delay;
        public float duration;
        public bool isSpeedBased;
    }
}
