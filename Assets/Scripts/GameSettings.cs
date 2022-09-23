using UnityEngine;
using UnityEngine.Events;

public static class GameSettings
{
    private const string SoundStateKey = "Sound_State";


    public static UnityAction<bool> OnSoundToggled;


    public static bool IsSoundEnabled
    {
        get => PlayerPrefs.GetInt(SoundStateKey, 1) == 1;
        private set => PlayerPrefs.SetInt(SoundStateKey, value ? 1 : 0);
    }


    public static void EnableSound()
    {
        ToggleSound(true);
    }

    public static void DisableSound()
    {
        ToggleSound(false);
    }


    private static void ToggleSound(bool isEnabled)
    {
        IsSoundEnabled = isEnabled;
        OnSoundToggled?.Invoke(isEnabled);
    }
}