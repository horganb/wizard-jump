using System;
using Levels;
using UnityEngine;

namespace Singletons
{
    [Serializable]
    public class Stage
    {
        public Sprite background;
        public Sprite platformLeft;
        public Sprite platformRight;
        public Sprite platformMiddle;
        public BossLevel bossLevel;
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