using System.Linq;
using Attacks;
using Interactable;
using UnityEngine;

namespace Singletons
{
    public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
    {
        public GameObject platformPrefab;
        public GameObject slimePrefab;
        public GameObject bigSlimePrefab;
        public GameObject waspPrefab;
        public GameObject chestPrefab;
        public GameObject choiceChestPrefab;
        public GameObject urnPrefab;

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

        private void GeneratePlatformLayer(Level level)
        {
            var centerX = Utils.RandomRangeAndSign(3f, 6f, 1);
            var centerY = _lastLocation.y + Random.Range(2f, 3f);
            _lastLocation = new Vector2(centerX, centerY);
            GenerateWaspWithChance(level.WaspChance, _lastLocation);
            if (Random.value <= 0.3f)
            {
                var leftPlatformLocation = new Vector2(Utils.RandomRangeWithPrecision(-6f, -2f, 1), centerY);
                var rightPlatformLocation = new Vector2(Utils.RandomRangeWithPrecision(2f, 6f, 1), centerY);
                PlacePlatform(leftPlatformLocation, Utils.RandomRangeWithPrecision(1f, 3f, 1));
                PlacePlatform(rightPlatformLocation, Utils.RandomRangeWithPrecision(1f, 3f, 1));
                GenerateSlimeWithChance(level.SlimeChance, level.BigSlimeChance, leftPlatformLocation);
                GenerateSlimeWithChance(level.SlimeChance, level.BigSlimeChance, rightPlatformLocation);
            }
            else
            {
                PlacePlatform(_lastLocation, Utils.RandomRangeWithPrecision(3f, 5f, 1));
                GenerateSlimeWithChance(level.SlimeChance, level.BigSlimeChance, _lastLocation);
            }
        }


        public void PlacePlatform(Vector2 location, float platformWidth, bool isReward = false)
        {
            var platform = Instantiate(platformPrefab, location, Quaternion.identity, transform);
            var platformComponent = platform.GetComponent<Platform>();
            platformComponent.SetWidth(platformWidth);
            platformComponent.isReward = isReward;
            var urnChance = Random.value;
            if (!isReward && urnChance < 0.1)
            {
                if (urnChance < 0.05)
                {
                    var urn1Position = location + Vector2.up * 1f + Vector2.left * 0.5f;
                    var urn2Position = location + Vector2.up * 1f + Vector2.right * 0.5f;
                    Instantiate(urnPrefab, urn1Position, Quaternion.identity, transform);
                    Instantiate(urnPrefab, urn2Position, Quaternion.identity, transform);
                }
                else
                {
                    var urnPosition = location + Vector2.up * 1f;
                    Instantiate(urnPrefab, urnPosition, Quaternion.identity, transform);
                }
            }
        }

        private void GenerateRewardPlatform(int levelNum)
        {
            _lastLocation = new Vector2(0f, _lastLocation.y + 1.5f);
            PlacePlatform(_lastLocation, 10f, true);
            var chestPosition = _lastLocation + Vector2.up * 1f;
            var chestObject = Instantiate(choiceChestPrefab, chestPosition, Quaternion.identity, gameObject.transform);
            var chest = chestObject.GetComponent<ChoiceChest>();
            if (levelNum == 1)
                chest.FillWithRandom<Attack>();
            else if (levelNum == 4)
                chest.FillWithRandom<Special.Special>();
            else
                chest.FillWithRandom<Gear.Gear>();
        }

        private void GenerateSlimeWithChance(float chance, float bigSlimeChance, Vector2 platformLocation)
        {
            if (IsTooNearRewardPlatform(platformLocation)) return;
            var startingPosition = platformLocation + Vector2.up * 1f + Vector2.right * Random.Range(-1f, 1f);
            var randomVal = Random.value;
            if (randomVal < chance)
            {
                var prefab = randomVal < bigSlimeChance ? bigSlimePrefab : slimePrefab;
                Instantiate(prefab, startingPosition, Quaternion.identity, gameObject.transform);
            }
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

        public void GenerateLevel(int levelNum)
        {
            Level[] levels =
            {
                new(size: 12),
                new(0.3f),
                new(0.5f, 0.0f, 0.1f),
                new(0.4f, 0.3f, 0.1f),
                new(0.6f, 0.3f, 0.3f),
                new(0.8f, 0.5f, 0.5f)
            };
            var level = levels[levelNum - 1];

            for (var i = 0; i < level.Size; i++)
                GeneratePlatformLayer(level);

            GenerateRewardPlatform(levelNum);
        }
    }
}