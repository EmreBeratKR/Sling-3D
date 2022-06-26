using Handle_System;
using UnityEngine;

namespace Goal_System
{
    public class InfectionGoal : Goal
    {
        [SerializeField] private InfectableHandle handle;


        public override bool IsAchieved => handle.IsInfected;
    }
}