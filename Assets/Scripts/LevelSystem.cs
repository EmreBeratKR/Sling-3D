using Helpers;

public class LevelSystem : Scenegleton<LevelSystem>
{
    public static LevelStatus LevelStatus
    {
        get => Instance.levelStatus;
        private set => Instance.levelStatus = value;
    }

    public static bool IsPlaying => LevelStatus == LevelStatus.Playing;
    public static bool IsLevelEnd => LevelStatus is LevelStatus.Failed or LevelStatus.Completed;
    
    
    private LevelStatus levelStatus;

    
    public void OnLevelStarted()
    {
        LevelStatus = LevelStatus.Playing;
    }

    public void OnSlingAttachedToPortal()
    {
        LevelStatus = LevelStatus.Completed;
    }
}

public enum LevelStatus
{
    WarmUp,
    Playing,
    Failed,
    Completed
}
