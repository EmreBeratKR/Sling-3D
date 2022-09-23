using UnityEngine;

namespace SoundSystem
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private bool playOnStart;


        private void Start()
        {
            TryPlayOnStart();
        }


        private void OnEnable()
        {
            GameSettings.OnSoundToggled += OnSoundToggled;
        }

        private void OnDisable()
        {
            GameSettings.OnSoundToggled -= OnSoundToggled;
        }


        public void ChangeAudioClip(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
        }
        

        private void OnSoundToggled(bool isEnabled)
        {
            ToggleMute(!isEnabled);
        }

        private void Play()
        {
            audioSource.Play();
        }

        private void Stop()
        {
            audioSource.Stop();
        }

        private void ToggleMute(bool isMuted)
        {
            audioSource.mute = isMuted;
        }

        private void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }

        private void TryPlayOnStart()
        {
            if (!playOnStart) return;
            
            Play();
            ToggleMute(!GameSettings.IsSoundEnabled);
        }
    }
}
