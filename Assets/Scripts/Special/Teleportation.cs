using Singletons;
using UnityEngine;

namespace Special
{
    public class Teleportation : Special
    {
        private const float ManaCost = 1f;

        public override string Name()
        {
            return "Teleportation";
        }

        public override void Cast(Vector2 worldPosition)
        {
            if (Player.Instance.mana < ManaCost) return;
            Player.Instance.mana -= ManaCost;
            Player.Instance.transform.position = worldPosition;
            Player.Instance.audioSource.PlayOneShot(AudioLibrary.Instance.teleport);
        }
    }
}