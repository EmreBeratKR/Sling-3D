using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform endAnchor;
    [SerializeField] private float motionInterval = 0.2f;
    [SerializeField] private float motionDuration = 0.5f;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool looping = true;


    [Header("Events")] 
    public UnityEvent onBeginMoveToStart;
    public UnityEvent onReachedToStart;
    public UnityEvent onBeginMoveToEnd;
    public UnityEvent onReachedToEnd;
    
    
    private Vector3 startPosition;
    private Vector3 endPosition;

    
    private void Start()
    {
        startPosition = transform.position;
        endPosition = endAnchor.position;

        if (playOnStart)
        {
            MoveToEnd();
        }
    }
    

    public void MoveToEnd()
    {
        transform.DOMove(endPosition, motionDuration)
            .SetDelay(motionInterval)
            .SetEase(Ease.InOutSine)
            .OnStart(() =>
            {
                onBeginMoveToEnd?.Invoke();
            })
            .OnComplete(() =>
            {
                onReachedToEnd?.Invoke();
                
                if (looping)
                {
                    MoveToStart();
                }
            });
    }

    public void MoveToStart()
    {
        transform.DOMove(startPosition, motionDuration)
            .SetDelay(motionInterval)
            .SetEase(Ease.InOutSine)
            .OnStart(() =>
            {
                onBeginMoveToStart?.Invoke();
            })
            .OnComplete(() =>
            {
                onReachedToStart?.Invoke();
                
                if (looping)
                {
                    MoveToEnd();
                }
            });
    }


#if UNITY_EDITOR

    [Button]
    private void ForceMoveStart()
    {
        transform.position -= endAnchor.rotation * endAnchor.localPosition;
        onBeginMoveToStart?.Invoke();
        onReachedToStart?.Invoke();
    }

    [Button]
    private void ForceMoveEnd()
    {
        transform.position += endAnchor.rotation * endAnchor.localPosition;
        onBeginMoveToEnd?.Invoke();
        onReachedToEnd?.Invoke();
    }
    
#endif
}
