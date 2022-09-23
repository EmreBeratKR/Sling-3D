using UnityEngine;

namespace Data_Container
{
    [CreateAssetMenu(menuName = "Data Containers/" + nameof(AudioClipContainer))]
    public class AudioClipContainer : Container<AudioClip>
    {
        
    }
}