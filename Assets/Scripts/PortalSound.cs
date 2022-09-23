using SoundSystem;
using UnityEngine;

public class PortalSound : MonoBehaviour
{
    [SerializeField] private SfxPlayer 
        portalOpeningSfxPlayer,
        portalSlingEnteringSfxPlayer;


    public void PlayOpening() => portalOpeningSfxPlayer.PlayRandomClip();

    public void PlaySlingEntering() => portalSlingEnteringSfxPlayer.PlayRandomClip();
}