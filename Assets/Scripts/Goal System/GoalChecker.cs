using System.Linq;
using ScriptableEvents.Core.Channels;
using UnityEngine;

namespace Goal_System
{
    public class GoalChecker : MonoBehaviour
    {
        [SerializeField] private VoidEventChannel allGoalsAchieved;
        
        private Goal[] goals;
        private bool goalsSearched;
        
        
        public void OnLevelLoaded()
        {
            SearchGoals();
        }
        
        public void OnGoalAchieved()
        {
            CheckAllGoals();
        }


        private void SearchGoals()
        {
            if (goalsSearched) return;
            
            goals = FindObjectsOfType<Goal>();
            goalsSearched = true;
        }

        private void CheckAllGoals()
        {
            if (!goalsSearched) return;
            
            var allAchieved = goals.All(goal => goal.IsAchieved);
            
            if (!allAchieved) return;
            
            allGoalsAchieved.RaiseEvent();
        }
    }
}
