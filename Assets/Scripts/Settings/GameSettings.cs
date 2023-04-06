using System;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
    [Serializable]
    public class GameSettings
    {
        [Range(0, 1)]
        public float AdditionalIncreaseDecreaseChance = .3f;
        public float CustomerSpeed = 3.5f;
        public float CustomerSpawnDelay = 4f;
        public int InitialBoosters = 1;
    }
}