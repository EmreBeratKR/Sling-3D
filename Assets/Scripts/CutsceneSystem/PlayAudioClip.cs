using UnityEngine;

namespace CutsceneSystem
{
    public class PlayAudioClip : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;


        public void Play()
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}