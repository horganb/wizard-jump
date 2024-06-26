using Enemies;
using UnityEngine;

namespace Singletons
{
    public class LevelManager : SingletonMonoBehaviour<LevelManager>
    {
        private int _currentLevel = 1;
        private bool _rewardPhase;

        public void PlayerOnPlatform(Platform platform)
        {
            Lava.Instance.isPaused = platform.isReward;
            var lastPlatform = Player.Instance.lastStandingPlatform;
            var lastPlatformWasReward = lastPlatform && lastPlatform.isReward;
            if (platform.isReward && platform != lastPlatform)
            {
                foreach (var pl in FindObjectsOfType<Platform>())
                    if (pl != platform)
                        Destroy(pl.gameObject);
                foreach (var enemy in FindObjectsOfType<Enemy>())
                    enemy.OnDie(Vector2.down);
            }

            if (!platform.isReward && lastPlatformWasReward) Destroy(lastPlatform.gameObject);
        }

        public void StartNextLevel()
        {
            _currentLevel++;
            Lava.Instance.growRate += 0.1f;
            LevelGenerator.Instance.GenerateLevel(_currentLevel);
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