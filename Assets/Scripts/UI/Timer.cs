using TMPro;
using UnityEngine;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerField;


        private State state;


        private void Start()
        {
            timerField.text = LevelSystem.CurrentLevelData.goldTime.ToString();
        }

        private void Update()
        {
            if (state == State.Disabled) return;
            
            timerField.text = LevelSystem.GoldSecondLeft.ToString();
        }


        public void OnLevelStarted()
        {
            Enable();
        }


        private void Enable()
        {
            if (state == State.Enabled) return;

            state = State.Enabled;
            timerField.gameObject.SetActive(true);
        }
        
        private void Disable()
        {
            if (state == State.Disabled) return;

            state = State.Disabled;
            timerField.gameObject.SetActive(false);
        }
        
        private enum State
        {
            Disabled,
            Enabled
        }
    }
}
