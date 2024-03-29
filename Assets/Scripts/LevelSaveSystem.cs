using System;
using System.Linq;
using Helpers;
using UnityEngine;

public static class LevelSaveSystem
{
    private const string LevelSaveKey = "Level_Save";
    private const string LevelSaveCreatedKey = "Level_Save_Created";


    private static bool IsLevelSaveCreated
    {
        get => PlayerPrefs.GetInt(LevelSaveCreatedKey, 0) == 1;
        set => PlayerPrefs.SetInt(LevelSaveCreatedKey, value ? 1 : 0);
    }
    
    
    public static void Save(LevelSave levelSave, int levelIndex)
    {
        var oldSave = Load();
        oldSave[levelIndex] = levelSave;
        JsonPrefs.SaveArray(LevelSaveKey, oldSave);
    }

    public static LevelSave[] Load()
    {
        if (!IsLevelSaveCreated)
        {
            IsLevelSaveCreated = true;
            return CreateLevelSave();
        }
        
        return JsonPrefs.LoadArray<LevelSave>(LevelSaveKey);
    }

    public static bool IsAllLevelsGoldTime()
    {
        var save = Load();
        return save.All(level => level.state == LevelState.CompletedGold);
    }


    private static LevelSave[] CreateLevelSave()
    {
        var levelSave = new LevelSave[LevelSystem.LevelCount];
        
        levelSave[0] = LevelSave.NewUnlocked;

        for (int i = 1; i < LevelSystem.LevelCount; i++)
        {
            levelSave[i] = LevelSave.Default;
        }
        
        JsonPrefs.SaveArray(LevelSaveKey,levelSave);

        return levelSave;
    }
}

[Serializable]
public struct LevelSave
{
    public static LevelSave Default => new LevelSave
    {
        state = LevelState.Locked,
        bestScore = 0
    };
    
    public static LevelSave NewUnlocked => new LevelSave
    {
        state = LevelState.NotPlayed,
        bestScore = 0
    };
    
    public LevelState state;
    public int bestScore;
}

public enum LevelState
{
    Locked,
    NotPlayed,
    CompletedNormal,
    CompletedGold
}