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
            LevelGenerator.Instance.PlacePlatform(worldPosition, 2f);
        }
    }
}