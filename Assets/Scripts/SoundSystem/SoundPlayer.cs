using UnityEngine;

namespace SoundSystem
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private bool playOnStart;


        private void Start()
        {
            ToggleMute(!GameSettings.IsSoundEnabled);
            TryPlayOnStart();
        }


        private void OnEnable()
        {
            GameSettings.OnSoundToggled += OnSoundToggled;
        }

        private void OnDestroy()
        {
            GameSettings.OnSoundToggled -= OnSoundToggled;
        }

        private void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }


        public void Play()
        {
            audioSource.Play();
        }

        public void Stop()
        {
            audioSource.Stop();
        }
        
        public void PlayClip(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            Play();
        }

        public void TryPlayClip(AudioClip audioClip)
        {
            if (audioSource.isPlaying) return;
            
            PlayClip(audioClip);
        }
        

        private void OnSoundToggled(bool isEnabled)
        {
            ToggleMute(!isEnabled);
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
        }
    }
}
