using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform endAnchor;
    [SerializeField] private float motionInterval;
    [SerializeField] private float motionDuration;
    
    private Vector3 startPosition;
    private Vector3 endPosition;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = endAnchor.position;

        MoveToEnd();
    }
    

    private void MoveToEnd()
    {
        transform.DOMove(endPosition, motionDuration)
            .SetDelay(motionInterval)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                MoveToStart();
            });
    }

    private void MoveToStart()
    {
        transform.DOMove(startPosition, motionDuration)
            .SetDelay(motionInterval)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                MoveToEnd();
            });
    }
}
