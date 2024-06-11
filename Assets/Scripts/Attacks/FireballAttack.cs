using Singletons;
using UnityEngine;

namespace Attacks
{
    public class FireballAttack : Attack
    {
        public override string Name()
        {
            return "Fireball";
        }

        public override GameObject GetPrefab()
        {
            return PrefabLibrary.Instance.fireball;
        }

        public override AudioClip GetClip()
        {
            return AudioLibrary.Instance.fireball;
        }
    }
}