using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundToggle : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;


        private void Start()
        {
            toggle.isOn = GameSettings.IsSoundEnabled;
        }


        public static void OnSoundToggled(bool isEnabled)
        {
            if (isEnabled)
            {
                GameSettings.EnableSound();
                return;
            }
            
            GameSettings.DisableSound();
        }
    }
}