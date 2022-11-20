using UnityEngine.Playables;

namespace CustomPlayables.Extensions
{
    public static class PlayableExtensions
    {
        public static float GetNormalizedTime(this Playable playable)
        {
            return (float) (playable.GetTime() / playable.GetDuration());
        }
    }
}