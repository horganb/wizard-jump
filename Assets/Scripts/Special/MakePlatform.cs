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
            if (Player.mana < ManaCost) return;
            Player.mana -= ManaCost;
            var levelGenerator = Object.FindObjectOfType<LevelGenerator>();
            var lastPlatform = Player.lastPlatform;
            var platformLevel = lastPlatform.level;
            if (lastPlatform.isReward) platformLevel += 1;
            levelGenerator.PlacePlatform(worldPosition, 2f, platformLevel);
        }
    }
}