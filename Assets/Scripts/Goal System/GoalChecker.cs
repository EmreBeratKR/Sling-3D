using System.Linq;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Goal_System
{
    public class GoalChecker : MonoBehaviour
    {
        [SerializeField] private VoidEventChannel allGoalsAchieved;
        
        private Goal[] goals;


        private void Awake()
        {
            goals = FindObjectsOfType<Goal>();
        }


        public void OnGoalAchieved()
        {
            CheckAllGoals();
        }


        private void CheckAllGoals()
        {
            var allAchieved = goals.All(goal => goal.IsAchieved);
            
            if (!allAchieved) return;
            
            allGoalsAchieved.RaiseEvent();
        }
    }
}
