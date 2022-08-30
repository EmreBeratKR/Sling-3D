using DG.Tweening;
using Handle_System;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace EnemySystem
{
    public class Grabber : MonoBehaviour
    {
        [Header("Event Channels")]
        [SerializeField] private HandleEventChannel onHandleGrabbed;
    
    
        [Header("References")] 
        [SerializeField] private Handle targetHandle;
        [SerializeField] private Transform grabAnchor;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform web;

        [Header("Values")]
        [SerializeField, Min(0f)] private float interval;
        [SerializeField, Min(0f)] private float duration;
        [SerializeField, Range(0f, 90f)] private float handAngle;
        [SerializeField, Min(0f)] private float handDuration;
        [SerializeField] private bool playOnStart = true;

        [Header("Debug")] 
        [SerializeField] private bool showHandGrab;


        private Vector3 LeftHandGrabEulerAngles => Vector3.forward * handAngle;
        private Vector3 LeftHandFreeEulerAngles => Vector3.zero;
        private Vector3 RightHandGrabEulerAngles => -LeftHandGrabEulerAngles;
        private Vector3 RightHandFreeEulerAngles => -LeftHandFreeEulerAngles;
        private Vector3 GrabPosition => grabAnchor.position;


        private Vector3 m_StartLocalPosition;
        private Vector3 m_EndLocalPosition;
        private Tween m_MoveTween;
        private bool m_IsGrabbed;


        private void Start()
        {
            m_StartLocalPosition = transform.localPosition;
            m_EndLocalPosition = m_StartLocalPosition + (targetHandle.Position - GrabPosition);
        
            UpdateWeb();

            if (playOnStart)
            {
                MoveToEnd();
            }
        }

#if UNITY_EDITOR
    
        private void OnValidate()
        {
            if (showHandGrab)
            {
                leftHand.eulerAngles = LeftHandGrabEulerAngles;
                rightHand.eulerAngles = RightHandGrabEulerAngles;
            }

            else
            {
                leftHand.localEulerAngles = Vector3.zero;
                rightHand.localEulerAngles = Vector3.zero;
            }
        }
    
#endif


        private void MoveToStart()
        {
            MoveTo(m_StartLocalPosition, 0f, MoveToEnd);
        }

        private void MoveToEnd()
        {
            MoveTo(m_EndLocalPosition, interval, ToggleTargetHandle);
        }

        private void GrabTargetHandle()
        {
            if (m_IsGrabbed) return;

            m_IsGrabbed = true;
        
            targetHandle.DisableHandle();
            onHandleGrabbed.RaiseEvent(targetHandle);
            RotateBothHand(LeftHandGrabEulerAngles, RightHandGrabEulerAngles, MoveToStart);
        }

        private void DropTargetHandle()
        {
            if (!m_IsGrabbed) return;

            m_IsGrabbed = false;
        
            targetHandle.EnableHandle();
            RotateBothHand(LeftHandFreeEulerAngles, RightHandFreeEulerAngles, MoveToStart);
        }

        private void ToggleTargetHandle()
        {
            if (m_IsGrabbed)
            {
                DropTargetHandle();
                return;
            }
        
            GrabTargetHandle();
        }
    
        private void MoveTo(Vector3 position, float delay, TweenCallback callback)
        {
            m_MoveTween?.Kill();
            m_MoveTween = transform.DOLocalMove(position, duration);

            m_MoveTween
                .SetDelay(delay)
                .SetEase(Ease.OutSine)
                .OnUpdate(() =>
                {
                    UpdateWeb();
                
                    if (m_IsGrabbed)
                    {
                        targetHandle.MoveTo(GrabPosition);
                    }
                })
                .onComplete = callback;
        }

        private void RotateBothHand(Vector3 leftHandEulerAngles, Vector3 rightHandEulerAngles, TweenCallback callback)
        {
            RotateHand(leftHand, leftHandEulerAngles);
            RotateHand(rightHand, rightHandEulerAngles, callback);
        }

        private void RotateHand(Transform hand, Vector3 eulerAngles, TweenCallback callback = null)
        {
            hand.DOLocalRotate(eulerAngles, handDuration)
                .SetEase(Ease.OutSine)
                .onComplete = callback;
        }

        private void UpdateWeb()
        {
            var length = m_StartLocalPosition.y - transform.localPosition.y;
            var webScale = web.localScale;
            webScale.y = length;
            web.localScale = webScale;
        }
    }
}