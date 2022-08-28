using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace PathSystem
{
    public class PathMover : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Path optionalPath;
    
        [Header("Values")]
        [SerializeField] private PathMoveDirection startDirection;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float interval = 0.2f;
        [SerializeField] private int startIndex = 0;
        [SerializeField] private bool playOnStart = true;


        private Path Path
        {
            get
            {
                if (optionalPath)
                {
                    return optionalPath;
                }

                if (!m_Path)
                {
                    m_Path = GetComponent<Path>();
                }

                return m_Path;
            }
        }

        private Vector3 CurrentPoint => Path[m_CurrentPointIndex];


        private Path m_Path;
        private Tween m_MoveTween;
        private PathMoveDirection m_Direction;
        private int m_CurrentPointIndex;
        private bool m_IsPlaying;


        private void Start()
        {
            m_CurrentPointIndex = startIndex;
            m_Direction = startDirection;

            if (playOnStart)
            {
                Play();
            }
        }


        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Play()
        {
            if (m_IsPlaying) return;

            m_IsPlaying = true;
            
            MoveToPoint();
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Stop()
        {
            if (!m_IsPlaying) return;

            m_IsPlaying = false;
            
            KillMoveTween();
        }
        
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Invert()
        {
            m_Direction = m_Direction.Inverted();

            if (m_Direction == PathMoveDirection.Forward)
            {
                DecrementPointIndex();
            }

            else
            {
                IncrementPointIndex();
            }

            if (m_IsPlaying)
            {
                MoveToPoint();
            }
        }
        

        private void MoveToPoint()
        {
            Vector3? point;
            
            if (m_Direction == PathMoveDirection.Forward)
            {
                point = Path.NextPoint(m_CurrentPointIndex);

                if (!point.HasValue) return;

                void OnNextPointReached()
                {
                    IncrementPointIndex();
                    MoveToPoint();
                }
                
                MoveTween(point.Value, OnNextPointReached);

                return;
            }

            point = Path.PreviousPoint(m_CurrentPointIndex);
            
            if (!point.HasValue) return;

            void OnPreviousPointReached()
            {
                DecrementPointIndex();
                MoveToPoint();
            }
            
            MoveTween(point.Value, OnPreviousPointReached);
        }

        private void MoveTween(Vector3 position, TweenCallback callback)
        {
            KillMoveTween();
            m_MoveTween = transform.DOMove(position, speed);

            m_MoveTween
                .SetDelay(interval)
                .SetSpeedBased()
                .SetEase(Ease.OutSine)
                .onComplete = callback;
        }

        private void KillMoveTween()
        {
            m_MoveTween?.Kill();
        }

        private void IncrementPointIndex()
        {
            m_CurrentPointIndex = Path.GetNextPointIndex(m_CurrentPointIndex);
        }

        private void DecrementPointIndex()
        {
            m_CurrentPointIndex = Path.GetPreviousPointIndex(m_CurrentPointIndex);
        }
        

#if UNITY_EDITOR

        private void OnValidate()
        {
            startIndex = Path.GetValidatedIndex(startIndex);
        }

        [Button]
        private void SnapToCurrentPoint()
        {
            if (Application.isEditor)
            {
                transform.position = Path[startIndex];
                return;
            }

            if (Application.isPlaying)
            {
                transform.position = CurrentPoint;
            }
        }

#endif
    }
}