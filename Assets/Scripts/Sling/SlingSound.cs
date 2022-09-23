using SoundSystem;
using UnityEngine;

namespace Sling
{
    public class SlingSound : MonoBehaviour
    {
        [SerializeField] private SfxPlayer 
            stretchSfxPlayer,
            flingSfxPlayer,
            bounceSfxPlayer,
            takeDamageSfxPlayer,
            grabSfxPlayer;


        public void PlayStretch() => stretchSfxPlayer.TryPlayRandomClip();

        public void PlayFling() => flingSfxPlayer.PlayRandomClip();

        public void PlayBounce() => bounceSfxPlayer.PlayRandomClip();

        public void PlayTakeDamage() => takeDamageSfxPlayer.PlayRandomClip();

        public void PlayGrab() => grabSfxPlayer.PlayRandomClip();
    }
}