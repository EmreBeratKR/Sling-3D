using System;
using System.Collections;
using Data_Container;
using Helpers;
using ScriptableEvents.Core.Channels;
using UnityEngine;

public class LevelSystem : Scenegleton<LevelSystem>
{
    public const int LevelCompleteScore = 500;
    
    private const string EnteredLevelKey = "Entered_Level";


    [Header("Event Channels")]
    [SerializeField] private VoidEventChannel levelLoaded;
    
    [Header("References")]
    [SerializeField] private LevelDataContainer levelDataContainer;
    [SerializeField] private Transform levelParent;
    
    
    public static LevelData CurrentLevelData { get; private set; }
    
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


    private int EnteredLevel
    {
        get => PlayerPrefs.GetInt(EnteredLevelKey, 0);
        set => PlayerPrefs.SetInt(EnteredLevelKey, value);
    }


    private LevelStatus levelStatus;
    private float startTime;
    private float completeTime;


    protected override void Awake()
    {
        base.Awake();
        LoadLevel();
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
    }


    private void LoadLevel()
    {
        CurrentLevelData = levelDataContainer[EnteredLevel];
        StartCoroutine(Loading());
        
        IEnumerator Loading()
        {
            yield return new WaitUntil(() => Instantiate(CurrentLevelData.prefab, levelParent));
            
            levelLoaded.RaiseEvent();
        }
    }
}

public enum LevelStatus
{
    WarmUp,
    Playing,
    Failed,
    Completed
}
