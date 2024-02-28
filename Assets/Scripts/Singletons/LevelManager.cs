using UnityEngine;

namespace Singletons
{
    public class LevelManager : SingletonMonoBehaviour<LevelManager>
    {
        private int _currentLevel = 1;

        public void PlayerOnPlatform(Platform platform)
        {
            if (platform.level > _currentLevel) StartLevel(platform.level);
            Lava.Instance.isPaused = platform.isReward;
            if (platform.isReward)
                foreach (var pl in FindObjectsOfType<Platform>())
                    if (pl.transform.position.y < platform.transform.position.y)
                        Destroy(pl.gameObject);
        }

        private void StartLevel(int level)
        {
            _currentLevel = level;
            Lava.Instance.growRate += 0.1f;
        }

        public void SkipToNextLevel()
        {
            var player = Player.Instance.gameObject;
            GameObject lowestPlatform = null;
            foreach (var platform in FindObjectsOfType<Platform>())
                if (platform.isReward && platform.transform.position.y >= player.transform.position.y + 1f &&
                    (lowestPlatform == null ||
                     platform.transform.position.y < lowestPlatform.transform.position.y))
                    lowestPlatform = platform.gameObject;

            if (lowestPlatform != null)
                player.transform.position = (Vector2)lowestPlatform.transform.position + Vector2.up;
        }
    }
}