using Singletons;
using UnityEngine;

namespace Special
{
    public class MakePlatform : Special
    {
        private const float ManaCost = 1f;

        public override string Name()
        {
            return "Make Platform";
        }

        public override void Cast(Vector2 worldPosition)
        {
            if (Player.Instance.mana < ManaCost) return;
            Player.Instance.mana -= ManaCost;
            var lastPlatform = Player.Instance.lastPlatform;
            var platformLevel = lastPlatform.level;
            if (lastPlatform.isReward) platformLevel += 1;
            LevelGenerator.Instance.PlacePlatform(worldPosition, 2f, platformLevel);
        }
    }
}