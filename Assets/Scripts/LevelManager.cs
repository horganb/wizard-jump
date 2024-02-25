using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int _currentLevel = 1;
    private Lava _lava;

    private void Start()
    {
        _lava = FindObjectOfType<Lava>();
    }

    public void PlayerOnPlatform(Platform platform)
    {
        if (platform.level > _currentLevel) StartLevel(platform.level);
        _lava.isPaused = platform.isReward;
        if (platform.isReward)
            foreach (var pl in FindObjectsOfType<Platform>())
                if (pl.transform.position.y < platform.transform.position.y)
                    Destroy(pl.gameObject);
    }

    private void StartLevel(int level)
    {
        _currentLevel = level;
        _lava.growRate += 0.1f;
    }

    public void SkipToNextLevel()
    {
        var player = FindObjectOfType<Player>().gameObject;
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