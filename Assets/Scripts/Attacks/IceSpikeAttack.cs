using Singletons;
using UnityEngine;

namespace Attacks
{
    public class IceSpikeAttack : Attack
    {
        public override string Name()
        {
            return "Ice Spike";
        }

        public override GameObject GetPrefab()
        {
            return PrefabLibrary.Instance.iceSpike;
        }

        public override AudioClip GetClip()
        {
            return AudioLibrary.Instance.iceSpike;
        }
    }
}