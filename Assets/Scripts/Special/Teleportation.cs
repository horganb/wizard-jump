using Singletons;
using UnityEngine;

namespace Special
{
    public class Teleportation : Special
    {
        public override string Name()
        {
            return "Teleportation";
        }

        public override void OnCast(Vector2 worldPosition)
        {
            Player.Instance.transform.position = worldPosition;
            Player.Instance.audioSource.PlayOneShot(AudioLibrary.Instance.teleport);
        }
    }
}