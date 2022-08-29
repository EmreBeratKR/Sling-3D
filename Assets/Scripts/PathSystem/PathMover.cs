using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace PathSystem
{
    public class PathMover : MonoBehaviour
    {
        private const string EventsFo = "Events";
        
        
        [Header("References")]
        [SerializeField] private Path optionalPath;
    
        [Header("Values")]
        [SerializeField] private PathMoveDirection startDirection;
        [SerializeField] private Ease moveEase = Ease.OutSine;
        [SerializeField] private float speed = 10f;
        [SerializeField] private float interval = 0.2f;
        [SerializeField] private int startIndex = 0;
        [SerializeField] private bool playOnStart = true;
        [SerializeField] private bool looping = true;


        [Foldout(EventsFo)] public UnityEvent
            onPlay,
            onPlaying,
            onStop,
            onInvert,
            onReachedPoint,
            onReachedPathEnd,
            onLapCompleted;


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
            onPlay?.Invoke();
            
            MoveToPoint();
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        public void Stop()
        {
            if (!m_IsPlaying) return;

            m_IsPlaying = false;
            onStop?.Invoke();
            
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
            
            onInvert?.Invoke();

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

                if (!point.HasValue)
                {
                    m_IsPlaying = looping;
                    onReachedPathEnd?.Invoke();

                    if (looping)
                    {
                        m_Direction = m_Direction.Inverted();
                        MoveToPoint();
                    }
                    
                    return;
                }

                void OnNextPointReached()
                {
                    IncrementPointIndex();

                    onReachedPoint?.Invoke();
                    
                    if (Path.IsFirstPoint(point.Value))
                    {
                        m_IsPlaying = looping;
                        onLapCompleted?.Invoke();

                        if (!looping) return;
                    }

                    MoveToPoint();
                }
                
                MoveTween(point.Value, OnNextPointReached);

                return;
            }

            point = Path.PreviousPoint(m_CurrentPointIndex);

            if (!point.HasValue)
            {
                m_IsPlaying = looping;
                onReachedPathEnd?.Invoke();

                if (looping)
                {
                    m_Direction = m_Direction.Inverted();
                    MoveToPoint();
                }
                
                return;
            }

            void OnPreviousPointReached()
            {
                DecrementPointIndex();
                
                onReachedPoint?.Invoke();
                
                if (Path.IsLastPoint(point.Value))
                {
                    m_IsPlaying = looping;
                    onLapCompleted?.Invoke();
                    
                    if (!looping) return;
                }
                
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
                .SetEase(moveEase)
                .OnUpdate(() =>
                {
                    onPlaying?.Invoke();
                })
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