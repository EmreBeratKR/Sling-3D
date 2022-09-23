using System;
using UnityEngine;

namespace Data_Container
{
    [CreateAssetMenu(menuName = "Data Containers/" + nameof(LevelDataContainer))]
    public class LevelDataContainer : Container<LevelData>
    {
        
    }

    [Serializable]
    public struct LevelData
    {
        public string tag;
        public string name;
        public GameObject prefab;
        public float goldTime;
        public LevelType type;
    }

    public enum LevelType
    {
        Normal,
        BossFight,
        Bonus
    }
}