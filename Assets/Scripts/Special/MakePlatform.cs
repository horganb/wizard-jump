using Singletons;
using UnityEngine;

namespace Special
{
    public class MakePlatform : Special
    {
        public override string Name()
        {
            return "Make Platform";
        }

        public override void OnCast(Vector2 worldPosition)
        {
            var lastPlatform = Player.Instance.lastPlatform;
            var platformLevel = lastPlatform.level;
            if (lastPlatform.isReward) platformLevel += 1;
            LevelGenerator.Instance.PlacePlatform(worldPosition, 2f, platformLevel);
        }
    }
}