using System;
using Data_Container;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelMap : MonoBehaviour
    {
        private const string NotPlayedYet = "Not played yet";
        private const string BestScorePrefix = "Best Score: ";
        
        
        [SerializeField] private LevelDataContainer levelDataContainer;
        [SerializeField] private LevelButtonSprites levelButtonSprites;
        [SerializeField] private TMP_Text levelNameField;
        [SerializeField] private TMP_Text levelInfoField;
        [SerializeField] private Transform levels;

        
        private LevelButton[] levelButtons;
        private LevelSave[] levelSave;
        private int selectedLevelIndex;


        private void Awake()
        {
            levelSave = LevelSaveSystem.Load();
            CacheLevelButtons();
        }

        private void Start()
        {
            UpdateAllLevelButtons();
            SelectLevelButton(selectedLevelIndex);
            UpdateLevelInfo(selectedLevelIndex);
        }


        public void OnLevelPointerEnter(LevelButton levelButton)
        {
            var levelIndex = levelButton.Index;
            UpdateLevelInfo(levelIndex);
        }

        public void OnLevelPointerExit()
        {
            UpdateLevelInfo(selectedLevelIndex);
        }

        public void OnLevelButtonClicked(LevelButton levelButton)
        {
            SelectLevelButton(levelButton.Index);
        }


        private void CacheLevelButtons()
        {
            levelButtons = new LevelButton[levels.childCount];
            
            for (int i = 0; i < levels.childCount; i++)
            {
                levelButtons[i] = levels.GetChild(i).GetComponent<LevelButton>();
            }
        }

        private void UpdateLevelButton(int buttonIndex)
        {
            var levelButton = levelButtons[buttonIndex];
            var levelState = levelSave[buttonIndex].state;
                
                
            if (levelState == LevelState.Locked)
            {
                levelButton.Disable();
                return;
            }

            var levelIcon = levelState switch
            {
                LevelState.CompletedNormal => levelButtonSprites.completedNormal,
                LevelState.CompletedGold => levelButtonSprites.completedGold,
                _ => levelButtonSprites.notPlayed
            };
                
            levelButton.UpdateIcon(levelIcon);
        }

        private void UpdateAllLevelButtons()
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                UpdateLevelButton(i);
            }
        }

        private void SelectLevelButton(int buttonIndex)
        {
            UpdateLevelButton(selectedLevelIndex);
            selectedLevelIndex = buttonIndex;
            levelButtons[selectedLevelIndex].UpdateIcon(levelButtonSprites.selected);
        }
        
        private void UpdateLevelInfo(int buttonIndex)
        {
            var levelNumber = buttonIndex + 1;
            var levelName = levelDataContainer[buttonIndex].name;
            levelNameField.text = $"Level {levelNumber}: {levelName}";

            var levelInfo = NotPlayedYet;
            
            var levelState = levelSave[buttonIndex].state;

            if (levelState is LevelState.CompletedNormal or LevelState.CompletedGold)
            {
                levelInfo = BestScorePrefix + levelSave[buttonIndex].bestScore;
            }

            levelInfoField.text = levelInfo;
        }
    }

    [Serializable]
    internal struct LevelButtonSprites
    {
        public Sprite notPlayed;
        public Sprite completedNormal;
        public Sprite completedGold;
        public Sprite selected;
    }
}
