using UnityEngine;

public class BonusLevel : MonoBehaviour
{
    private const string BonusLevelScoreKey = "Bonus_Level_Score";
    private const int CompleteScore = 1000;


    public void OnComplete()
    {
        var timeLeft = LevelSystem.GoldTimeLeft;
        var timeBonus = Mathf.RoundToInt(timeLeft * 10);
        var score = CompleteScore + timeBonus;
        
        SetBonusLevelScore(score);
    }
    

    public static int GetBonusLevelScore()
    {
        return PlayerPrefs.GetInt(BonusLevelScoreKey, 0);
    }
    
    
    private static void SetBonusLevelScore(int value)
    {
        PlayerPrefs.SetInt(BonusLevelScoreKey, value);
    }
}