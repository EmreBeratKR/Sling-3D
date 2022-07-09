using ScriptableEvents.Core.Channels;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        private const int MaxLifeCount = 5;

        [Header("Event Channels")]
        [SerializeField] private VoidEventChannel slingDied;
        
        [Header("References")]
        [SerializeField] private Image[] bars;
        
        [Header("Values")]
        [SerializeField] private Color activeColor;
        [SerializeField] private Color deActiveColor;
        
        private int livesLeft;


        private void Start()
        {
            livesLeft = MaxLifeCount;
            UpdateBars();
        }

        public void OnSlingLostLife()
        {
            livesLeft--;
            livesLeft = Mathf.Max(0, livesLeft);
            UpdateBars();
            CheckIfDead();
        }


        private void UpdateBars()
        {
            for (int i = 0; i < bars.Length; i++)
            {
                bars[i].color = livesLeft > i ? activeColor : deActiveColor;
            }
        }

        private void CheckIfDead()
        {
            if (livesLeft > 0) return;
            
            slingDied.RaiseEvent();
        }
    }
}
