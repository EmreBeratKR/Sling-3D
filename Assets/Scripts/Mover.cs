using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform endAnchor;
    [SerializeField] private float motionInterval = 0.2f;
    [SerializeField] private float motionDuration = 0.5f;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool looping = true;
    
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
            .OnComplete(() =>
            {
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
            .OnComplete(() =>
            {
                if (looping)
                {
                    MoveToEnd();
                }
            });
    }
}
