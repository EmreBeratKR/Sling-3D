using EnemySystem;
using UnityEngine;

namespace Goal_System
{
    public class KillGoal : Goal
    {
        [SerializeField] private Enemy enemy;


        public override bool IsAchieved => enemy.IsDead;
    }
}