using System;
using UnityEngine;

namespace Singletons
{
    [Serializable]
    public class Stage
    {
        public GameObject bossLevel;
        public EnemyType[] enemies;
        public Level[] levels;

        [Serializable]
        public class EnemyType
        {
            public GameObject prefab;
            public float power;
            public int maxPerPlatform;
        }
    }
}