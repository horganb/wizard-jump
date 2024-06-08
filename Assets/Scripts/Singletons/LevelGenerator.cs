using System.Linq;
using UnityEngine;

namespace Singletons
{
    public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
    {
        public GameObject platformPrefab;
        public GameObject slimePrefab;
        public GameObject waspPrefab;
        public GameObject chestPrefab;

        private Vector2 _lastLocation;

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            _lastLocation = Vector2.zero;
            PlacePlatform(Vector2.zero, 4f);
            GenerateLevel(1);
        }

        private Platform GeneratePlatform(float xVariation, float yVariation, float platformWidth)
        {
            _lastLocation = new Vector2(_lastLocation.x + xVariation,
                _lastLocation.y + yVariation);
            return PlacePlatform(_lastLocation, platformWidth);
        }

        private void GeneratePlatformLayer(float slimeChance, float waspChance)
        {
            var centerX = Utils.RandomRangeAndSign(3f, 6f);
            var centerY = _lastLocation.y + Random.Range(2f, 3f);
            _lastLocation = new Vector2(centerX, centerY);
            GenerateWaspWithChance(waspChance, _lastLocation);
            if (Random.value <= 0.3f)
            {
                var leftPlatformLocation = new Vector2(centerX + Random.Range(-4f, -2f), centerY);
                var rightPlatformLocation = new Vector2(centerX + Random.Range(2f, 4f), centerY);
                PlacePlatform(leftPlatformLocation, Random.Range(1f, 3f));
                PlacePlatform(rightPlatformLocation, Random.Range(1f, 3f));
                GenerateSlimeWithChance(slimeChance, leftPlatformLocation);
                GenerateSlimeWithChance(slimeChance, rightPlatformLocation);
            }
            else
            {
                PlacePlatform(_lastLocation, Random.Range(3f, 5f));
                GenerateSlimeWithChance(slimeChance, _lastLocation);
            }
        }


        public Platform PlacePlatform(Vector2 location, float platformWidth)
        {
            var platform = Instantiate(platformPrefab, location, Quaternion.identity, transform);
            var platformComponent = platform.GetComponent<Platform>();
            platformComponent.SetWidth(platformWidth);
            return platformComponent;
        }

        private void GenerateRewardPlatform()
        {
            _lastLocation.x = 0f;
            var platform = GeneratePlatform(0f, 1.5f, 10f);
            platform.isReward = true;
            var chestPosition = _lastLocation + Vector2.up * 1f;
            Instantiate(chestPrefab, chestPosition, Quaternion.identity, gameObject.transform);
        }

        private void GenerateSlimeWithChance(float chance, Vector2 platformLocation)
        {
            if (IsTooNearRewardPlatform(platformLocation)) return;
            var startingPosition = platformLocation + Vector2.up * 1f + Vector2.right * Random.Range(-1f, 1f);
            if (Random.value < chance)
                Instantiate(slimePrefab, startingPosition, Quaternion.identity, gameObject.transform);
        }

        private void GenerateWaspWithChance(float chance, Vector2 platformLocation)
        {
            if (IsTooNearRewardPlatform(platformLocation)) return;
            var startingPosition = platformLocation + Random.insideUnitCircle * 8f;
            if (Random.value < chance)
                Instantiate(waspPrefab, startingPosition, Quaternion.identity, gameObject.transform);
        }

        private bool IsTooNearRewardPlatform(Vector2 platformLocation)
        {
            return FindObjectsOfType<Platform>().Any(platform =>
            {
                var rewardY = platform.transform.position.y;
                return platform.isReward && platformLocation.y >= rewardY && platformLocation.y <= rewardY + 8f;
            });
        }

        public void GenerateLevel(int level)
        {
            switch (level)
            {
                case 1:
                    for (var i = 0; i < 30; i++) GeneratePlatformLayer(0.3f, 0f);

                    break;
                case 2:
                    for (var i = 0; i < 30; i++) GeneratePlatformLayer(0.5f, 0f);

                    break;
                case 3:
                    for (var i = 0; i < 30; i++) GeneratePlatformLayer(0.4f, 0.3f);

                    break;
                case 4:
                    for (var i = 0; i < 40; i++) GeneratePlatformLayer(0.6f, 0.3f);

                    break;
            }

            GenerateRewardPlatform();
        }
    }
}