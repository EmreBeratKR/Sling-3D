using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelCompletePanel : MonoBehaviour
    {
        private const string LevelCompletePrefix = "Level Complete: ";
        private const string TimeLeftPrefix = "Time Left: ";
        private const string TimeBonusPrefix = "Time Bonus: ";
        private const string LevelScorePrefix = "Level Score: ";
    
        [SerializeField] private GameObject main;
        [SerializeField] private TMP_Text levelCompleteField;
        [SerializeField] private TMP_Text timeLeftField;
        [SerializeField] private TMP_Text timeBonusField;
        [SerializeField] private TMP_Text levelScoreField;


        public void OnLevelCompleted()
        {
            StartCoroutine(DelayedResponse());

            IEnumerator DelayedResponse()
            {
                yield return new WaitForSeconds(1f);
                UpdateSummary();
                Enable();
            }
        }


        private void Enable()
        {
            main.SetActive(true);
        }
    
        private void Disable()
        {
            main.SetActive(false);
        }

        private void UpdateSummary()
        {
            var levelCompleteScore = LevelSystem.LevelCompleteScore;
            levelCompleteField.text = LevelCompletePrefix + levelCompleteScore;
        
            var timeLeft = LevelSystem.GoldTimeLeft;
            timeLeftField.text = TimeLeftPrefix + timeLeft;
        
            var timeBonus = Mathf.RoundToInt(timeLeft * 10);
            timeBonusField.text = TimeBonusPrefix + timeBonus;

            var levelScore = levelCompleteScore + timeBonus; 
            levelScoreField.text = LevelScorePrefix + levelScore;
            
            var isGoldTime = timeLeft >= 0.1f;

            var completedLevelIndex = LevelSystem.SelectedLevel;
            var oldSave = LevelMap.LevelSave[completedLevelIndex];

            if (oldSave.bestScore < levelScore)
            {
                oldSave.state = isGoldTime ? LevelState.CompletedGold : LevelState.CompletedNormal;
                oldSave.bestScore = levelScore;
                LevelSaveSystem.Save(oldSave, completedLevelIndex);
            }

            if (LevelSystem.IsLastLevel(completedLevelIndex))
            {
                Debug.Log("Congratz, You have Completed all the Levels!");
            }

            else
            {
                var nextLevelIndex = completedLevelIndex + 1;
                var nextLevelOldSave = LevelMap.LevelSave[nextLevelIndex];
                LevelSystem.SelectedLevel++;

                if (nextLevelOldSave.state == LevelState.Locked)
                {
                    nextLevelOldSave = LevelSave.NewUnlocked;
                    LevelSaveSystem.Save(nextLevelOldSave, nextLevelIndex);
                }
            }
        }
    }
}
