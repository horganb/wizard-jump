using UnityEngine;

namespace Singletons
{
    public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
    {
        public GameObject platformPrefab;
        public GameObject slimePrefab;
        public GameObject waspPrefab;
        public GameObject chestPrefab;

        private int _currentLevel = 1;
        private Vector2 _lastLocation;

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            _lastLocation = Vector2.zero;
            PlacePlatform(Vector2.zero, 4f, _currentLevel);
            GenerateLevel1();
            GenerateLevel2();
            GenerateLevel3();
            GenerateLevel4();
        }

        private Platform GeneratePlatform(float xVariation, float yVariation, float platformWidth)
        {
            _lastLocation = new Vector2(_lastLocation.x + xVariation,
                _lastLocation.y + yVariation);
            return PlacePlatform(_lastLocation, platformWidth, _currentLevel);
        }


        public Platform PlacePlatform(Vector2 location, float platformWidth, int level)
        {
            var platform = Instantiate(platformPrefab, location, Quaternion.identity, transform);
            var platformComponent = platform.GetComponent<Platform>();
            platformComponent.SetWidth(platformWidth);
            platformComponent.level = level;
            return platformComponent;
        }

        private void GenerateRewardPlatform()
        {
            var platform = GeneratePlatform(0f, 1.5f, 10f);
            platform.isReward = true;
            var chestPosition = _lastLocation + Vector2.up * 1f;
            Instantiate(chestPrefab, chestPosition, Quaternion.identity, gameObject.transform);
        }

        private void GenerateSlimeWithChance(float chance)
        {
            var startingPosition = _lastLocation + Vector2.up * 1f + Vector2.right * Random.Range(-1f, 1f);
            if (Random.value < chance)
                Instantiate(slimePrefab, startingPosition, Quaternion.identity, gameObject.transform);
        }

        private void GenerateWaspWithChance(float chance)
        {
            var startingPosition = _lastLocation + Random.insideUnitCircle * 8f;
            if (Random.value < chance)
                Instantiate(waspPrefab, startingPosition, Quaternion.identity, gameObject.transform);
        }

        private void GenerateLevel1()
        {
            for (var i = 0; i < 30; i++)
            {
                var platformXVariation = Utils.RandomRangeAndSign(3f, 8f);
                var platformYVariation = Random.Range(1.5f, 2f);
                var platformWidth = Random.Range(2f, 6f);
                GeneratePlatform(platformXVariation, platformYVariation, platformWidth);
                GenerateSlimeWithChance(0.15f);
            }

            GenerateRewardPlatform();
        }

        private void GenerateLevel2()
        {
            _currentLevel = 2;
            for (var i = 0; i < 30; i++)
            {
                var platformXVariation = Utils.RandomRangeAndSign(2f, 8f);
                var platformYVariation = Random.Range(1.5f, 2.5f);
                var platformWidth = Random.Range(0.5f, 5f);
                GeneratePlatform(platformXVariation, platformYVariation, platformWidth);
                GenerateSlimeWithChance(0.4f);
            }

            GenerateRewardPlatform();
        }

        private void GenerateLevel3()
        {
            _currentLevel = 3;
            for (var i = 0; i < 30; i++)
            {
                var platformXVariation = Utils.RandomRangeAndSign(1f, 8f);
                var platformYVariation = Random.Range(1.5f, 2.5f);
                var platformWidth = Random.Range(0.5f, 4f);
                GeneratePlatform(platformXVariation, platformYVariation, platformWidth);
                GenerateSlimeWithChance(0.4f);
                GenerateWaspWithChance(0.2f);
            }

            GenerateRewardPlatform();
        }

        private void GenerateLevel4()
        {
            _currentLevel = 4;
            for (var i = 0; i < 40; i++)
            {
                var platformXVariation = Utils.RandomRangeAndSign(1f, 8f);
                var platformYVariation = Random.Range(1.5f, 2.5f);
                var platformWidth = Random.Range(0.5f, 4f);
                GeneratePlatform(platformXVariation, platformYVariation, platformWidth);
                GenerateSlimeWithChance(0.3f);
                GenerateSlimeWithChance(0.3f);
                GenerateWaspWithChance(0.2f);
                GenerateWaspWithChance(0.2f);
            }

            GenerateRewardPlatform();
        }
    }
}