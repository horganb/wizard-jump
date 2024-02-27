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
            if (Player.mana < ManaCost) return;
            Player.mana -= ManaCost;
            Player.transform.position = worldPosition;
            Player.audioSource.PlayOneShot(AudioLibrary.teleport);
        }
    }
}