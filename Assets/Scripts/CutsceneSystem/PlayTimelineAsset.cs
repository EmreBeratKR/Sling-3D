using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CutsceneSystem
{
    public class PlayTimelineAsset : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        [SerializeField] private TimelineAsset timelineAsset;


        public void Play()
        {
            director.Play(timelineAsset);
        }
    }
}