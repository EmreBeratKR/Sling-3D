using Data_Container;
using UnityEngine;

namespace SoundSystem
{
    public class SfxPlayer : SoundPlayer
    {
        [SerializeField] private AudioClipContainer audioClipContainer;


        private AudioClip RandomClip => audioClipContainer.Random;
        

        public void PlayRandomClip()
        {
            PlayClip(RandomClip);
        }

        public void TryPlayRandomClip()
        {
            TryPlayClip(RandomClip);
        }
    }
}