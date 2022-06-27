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
        }
    }
}
