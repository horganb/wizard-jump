using Singletons;
using UnityEngine;

namespace Special
{
    public class Saw : Special
    {
        public override string Name()
        {
            return "Platform Saw";
        }

        public override void OnCast(Vector2 worldPosition)
        {
            Vector2 playerPosition = Player.Instance.transform.position;
            var directionVector = (worldPosition - playerPosition).normalized;
            var startPosition = playerPosition + directionVector * 0.5f;
            var rotation = Quaternion.FromToRotation(Vector2.right, directionVector);
            Object.Instantiate(PrefabLibrary.Instance.saw, startPosition, rotation);
        }
    }
}