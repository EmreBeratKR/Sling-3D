using System;
using System.Collections;
using CutsceneSystem;
using Data_Container;
using Helpers;
using ScriptableEvents.Core.Channels;
using SoundSystem;
using UnityEngine;

public class LevelSystem : Scenegleton<LevelSystem>
{
    public const int LevelCompleteScore = 500;
    public const int LevelCount = 50;
    
    private const string EnteredLevelKey = "Entered_Level";


    [Header("Event Channels")]
    [SerializeField] private VoidEventChannel levelLoaded;
    
    [Header("References")]
    [SerializeField] private LevelDataContainer levelDataContainer;
    [SerializeField] private AudioClipContainer levelAudioClipContainer;
    [SerializeField] private SoundPlayer levelSoundPlayer;
    [SerializeField] private Transform levelParent;

    [Header("Values")]
    [SerializeField] private bool isMainMenu;
    [SerializeField] private bool isCutscene;
    [SerializeField] private bool isBonusLevel;
    
    
    public static LevelData CurrentLevelData { get; private set; }
    
    
    public static int SelectedLevel
    {
        get => PlayerPrefs.GetInt(EnteredLevelKey, 0);
        set => PlayerPrefs.SetInt(EnteredLevelKey, value);
    }
    
    public static LevelStatus LevelStatus
    {
        get => Instance.levelStatus;
        private set => Instance.levelStatus = value;
    }

    public static int ElapsedSecond => Mathf.FloorToInt(ElapsedTime);

    public static float ElapsedTime
    {
        get
        {
            var time = LevelStatus == LevelStatus.Completed ? Instance.completeTime : Time.time;
            return time - Instance.startTime;
        }
    }

    public static int GoldSecondLeft => Mathf.FloorToInt(GoldTimeLeft);
    
    public static float GoldTimeLeft
    {
        get
        {
            var timeLeft = Mathf.Max(0f, CurrentLevelData.goldTime - ElapsedTime);
            return (float)Math.Round(timeLeft, 1);
        }
    }

    public static bool IsPlaying => LevelStatus == LevelStatus.Playing;
    public static bool IsLevelEnd => LevelStatus is LevelStatus.Failed or LevelStatus.Completed;


    private AudioClip LevelCompleteAudioClip => levelAudioClipContainer[3];
    
    
    private LevelStatus levelStatus;
    private float startTime;
    private float completeTime;


    protected override void Awake()
    {
        base.Awake();

        if (isMainMenu)
        {
            levelLoaded.RaiseEvent();
            return;
        }
        
        if (isCutscene) return;

        if (isBonusLevel)
        {
            LoadBonusLevel();
            return;
        }
        
        LoadLevel();
    }

    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }


    public void OnLevelStarted()
    {
        LevelStatus = LevelStatus.Playing;
        startTime = Time.time;
    }

    public void OnSlingAttachedToPortal()
    {
        LevelStatus = LevelStatus.Completed;
        completeTime = Time.time;
        levelSoundPlayer.PlayClip(LevelCompleteAudioClip);
    }

    public static bool IsLastLevel(int levelIndex)
    {
        var level = levelIndex + 1;
        return level == LevelCount;
    }

    public static void LoadCutscene(int index)
    {
        SceneController.LoadCutScene(index);
    }


    private void OnCutsceneInitialized(CutsceneEventResponse response)
    {
        levelStatus = LevelStatus.Playing;
    }
    

    private void LoadLevel()
    {
        CurrentLevelData = levelDataContainer[SelectedLevel];
        var levelAudioClip = levelAudioClipContainer[(int) CurrentLevelData.type];
        levelSoundPlayer.PlayClip(levelAudioClip);
        StartCoroutine(Loading());
        
        IEnumerator Loading()
        {
            yield return new WaitUntil(() => Instantiate(CurrentLevelData.prefab, levelParent));
            
            levelLoaded.RaiseEvent();
        }
    }

    private void LoadBonusLevel()
    {
        CurrentLevelData = new LevelData
        {
            goldTime = 170f,
            name = "Gold Level",
            type = LevelType.Bonus
        };
        
        var levelAudioClip = levelAudioClipContainer[(int) CurrentLevelData.type];
        levelSoundPlayer.PlayClip(levelAudioClip);
        StartCoroutine(Routine());

        IEnumerator Routine()
        {
            yield return null;
            
            levelLoaded.RaiseEvent();
        }
    }

    private void AddListeners()
    {
        CutsceneManager.OnCutsceneInitialized += OnCutsceneInitialized;
    }

    private void RemoveListeners()
    {
        CutsceneManager.OnCutsceneInitialized -= OnCutsceneInitialized;
    }
}

public enum LevelStatus
{
    WarmUp,
    Playing,
    Failed,
    Completed
}
