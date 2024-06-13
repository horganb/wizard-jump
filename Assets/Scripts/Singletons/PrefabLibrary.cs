using UnityEngine;

namespace Singletons
{
    public class PrefabLibrary : SingletonMonoBehaviour<PrefabLibrary>
    {
        public GameObject healthDrop;
        public GameObject orbDrop;
        public GameObject fireball;
        public GameObject iceSpike;
        public GameObject bomb;
        public GameObject saw;
    }
}