using ScriptableEvents.Core.Channels;
using TMPro;
using UnityEngine;

namespace Goal_System
{
    public class CoinGoal : Goal
    {
        [SerializeField] private VoidEventChannel goalAchieved;
        [SerializeField] private TMP_Text counter;
        [SerializeField] private int count;


        private int m_CollectedCount;


        public override bool IsAchieved => m_CollectedCount >= count;


        private void Start()
        {
            SetCounter(m_CollectedCount);
        }


        public void AddCoin(int value)
        {
            m_CollectedCount += value;
            SetCounter(m_CollectedCount);

            if (IsAchieved)
            {
                goalAchieved.RaiseEvent();
            }
        }


        private void SetCounter(int value)
        {
            counter.text = $"{value} / {count}";
        }
    }
}