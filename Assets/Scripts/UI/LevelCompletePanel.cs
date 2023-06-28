using System.Collections;
using SoundSystem;
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
        private const float Delay = 1f;
        
        [SerializeField] private GameObject main;
        [SerializeField] private TMP_Text levelCompleteField;
        [SerializeField] private TMP_Text timeLeftField;
        [SerializeField] private TMP_Text timeBonusField;
        [SerializeField] private TMP_Text levelScoreField;
        [SerializeField] private GameObject goldTimeText;
        [SerializeField] private GameObject nextButton;
        [SerializeField] private SoundPlayer soundPlayer;
        [SerializeField] private SoundPlayer soundPlayerAlt;
        [SerializeField] private AudioClip[] soundClips;


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

            if (isGoldTime)
            {
                PlayGoldTime();
            }

            else
            {
                PlayNormalTime();
            }
            
            if (oldSave.bestScore < levelScore)
            {
                oldSave.state = isGoldTime ? LevelState.CompletedGold : LevelState.CompletedNormal;
                oldSave.bestScore = levelScore;
                LevelSaveSystem.Save(oldSave, completedLevelIndex);
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

        private void PlayNormalTime()
        {
            goldTimeText.SetActive(false);
            levelCompleteField.gameObject.SetActive(false);
            timeLeftField.gameObject.SetActive(false);
            timeBonusField.gameObject.SetActive(false);
            levelScoreField.gameObject.SetActive(false);
            nextButton.SetActive(false);

            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(Delay);
                
                levelCompleteField.gameObject.SetActive(true);
                soundPlayer.PlayClip(soundClips[0]);

                yield return new WaitForSeconds(Delay);
                
                timeLeftField.gameObject.SetActive(true);
                timeBonusField.gameObject.SetActive(true);
                
                yield return new WaitForSeconds(Delay);
                
                levelScoreField.gameObject.SetActive(true);
                nextButton.SetActive(true);
            }
        }

        private void PlayGoldTime()
        {
            goldTimeText.SetActive(false);
            levelCompleteField.gameObject.SetActive(false);
            timeLeftField.gameObject.SetActive(false);
            timeBonusField.gameObject.SetActive(false);
            levelScoreField.gameObject.SetActive(false);
            nextButton.SetActive(false);

            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                yield return new WaitForSeconds(Delay);
                
                levelCompleteField.gameObject.SetActive(true);
                soundPlayer.PlayClip(soundClips[0]);

                yield return new WaitForSeconds(Delay);
                
                timeLeftField.gameObject.SetActive(true);
                timeBonusField.gameObject.SetActive(true);
                soundPlayer.PlayClip(soundClips[1]);
                
                yield return new WaitForSeconds(Delay);
                
                goldTimeText.SetActive(true);
                levelScoreField.gameObject.SetActive(true);
                nextButton.SetActive(true);
                soundPlayer.PlayClip(soundClips[2]);
                soundPlayerAlt.PlayClip(soundClips[3]);
            }
        }
    }
}
