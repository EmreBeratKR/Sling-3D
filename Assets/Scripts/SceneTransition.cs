using DG.Tweening;
using Helpers;
using SoundSystem;
using UnityEngine;

public class SceneTransition : Singleton<SceneTransition>
{
    [SerializeField] private RectTransform slimeImage;
    [SerializeField] private SoundPlayer slimeAudio;
    
    
    public static void FadeInOutSlime()
    {
        const float verticalStretch = 2.5f;
        const float duration = 0.810f;
        
        Instance.slimeImage.DOAnchorPosY(-3000 * verticalStretch, 0.810f)
            .From(Vector2.up * 1130);

        Instance.slimeImage.DOScaleY(verticalStretch, duration)
            .From(Vector3.one);
        
        Instance.slimeAudio.Play();
    }
}