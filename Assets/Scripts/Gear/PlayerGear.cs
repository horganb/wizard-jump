using Singletons;
using UnityEngine;

namespace Gear
{
    public class PlayerGear : SingletonMonoBehaviour<PlayerGear>
    {
        public GameObject boots;
        public GameObject goggles;
        public GameObject greaves;
        public GameObject gloves;
        public GameObject armor;
    }
}