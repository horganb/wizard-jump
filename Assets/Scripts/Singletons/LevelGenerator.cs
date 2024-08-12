using System;
using System.Collections.Generic;
using System.Linq;
using Interactable;
using Level;
using Levels;
using RewardType;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Singletons
{
    public class LevelGenerator : SingletonMonoBehaviour<LevelGenerator>
    {
        public GameObject platformPrefab;
        public GameObject flagPrefab;
        public GameObject choiceChestPrefab;
        public GameObject urnPrefab;
        public GameStages stages;

        private Vector2 _lastLocation;

        private void Start()
        {
            Generate();
        }

        private void Generate()
        {
            _lastLocation = Vector2.zero;
            PlacePlatform(Vector2.zero, 4f);
            GenerateLevel(1, 1);
        }

        private void GeneratePlatformLayer()
        {
            var centerX = Utils.RandomRangeAndSign(3f, 6f, 1);
            var centerY = _lastLocation.y + Random.Range(2f, 3f);
            _lastLocation = new Vector2(centerX, centerY);
            if (Random.value <= 0.3f)
            {
                var leftPlatformLocation = new Vector2(Utils.RandomRangeWithPrecision(-6f, -2f, 1), centerY);
                var rightPlatformLocation = new Vector2(Utils.RandomRangeWithPrecision(2f, 6f, 1), centerY);
                PlacePlatform(leftPlatformLocation, Utils.RandomRangeWithPrecision(1f, 3f, 1));
                PlacePlatform(rightPlatformLocation, Utils.RandomRangeWithPrecision(1f, 3f, 1));
            }
            else
            {
                PlacePlatform(_lastLocation, Utils.RandomRangeWithPrecision(3f, 5f, 1));
            }
        }


        public void PlacePlatform(Vector2 location, float platformWidth, bool isReward = false)
        {
            var platform = Instantiate(platformPrefab, location, Quaternion.identity, transform);
            var platformComponent = platform.GetComponent<Platform>();
            platformComponent.SetWidth(platformWidth);
            platformComponent.isReward = isReward;
        }

        private void GenerateRewardPlatform(bool isFirstLevel)
        {
            _lastLocation = new Vector2(0f, _lastLocation.y + 1.5f);
            PlacePlatform(_lastLocation, 10f, true);
            var chests = GenerateRewardChests(isFirstLevel);
            AssignRewardsToChests(isFirstLevel, chests);
            Instantiate(flagPrefab, _lastLocation + Vector2.left * 5f, Quaternion.identity, transform);
        }

        private List<ChoiceChest> GenerateRewardChests(bool isFirstLevel)
        {
            List<ChoiceChest> chests = new();
            var numChests = isFirstLevel ? 1 : Random.Range(2, 4);
            foreach (var offset in Utils.GetIntervalsAroundZero(numChests, 2.5f))
            {
                var chestPosition = _lastLocation + Vector2.up + Vector2.right * offset;
                var chestObject = Instantiate(choiceChestPrefab, chestPosition, Quaternion.identity, transform);
                chests.Add(chestObject.GetComponent<ChoiceChest>());
            }

            return chests;
        }

        private void AssignRewardsToChests(bool isFirstLevel, List<ChoiceChest> chests)
        {
            List<Type> rewardTypesUsed = new();
            foreach (var chest in chests)
            {
                RewardType.RewardType rewardType;
                if (isFirstLevel)
                {
                    rewardType = chest.GetComponentInChildren<LearnSpell>(true);
                }
                else
                {
                    var possibleRewardTypes = chest.GetComponentsInChildren<RewardType.RewardType>(true)
                        .Where(r => r.CanSpawn() && !rewardTypesUsed.Contains(r.GetType()));
                    rewardType = Utils.RandomFromArray(possibleRewardTypes.ToArray());
                }

                rewardTypesUsed.Add(rewardType.GetType());
                rewardType.gameObject.SetActive(true);
                chest.rewardType = rewardType;
            }
        }

        private bool IsTooNearRewardPlatform(Vector2 platformLocation)
        {
            return FindObjectsOfType<Platform>().Any(platform =>
            {
                var rewardY = platform.transform.position.y;
                return platform.isReward && platformLocation.y >= rewardY - 5f && platformLocation.y <= rewardY + 5f;
            });
        }


        private void SpawnEnemies(float totalPower, Stage.EnemyType[] types)
        {
            var chosenEnemyTypes = Utils.RandomFromArray(types, Random.Range(1, 4));
            var platforms = FindObjectsOfType<Platform>().Where(
                p => !IsTooNearRewardPlatform(p.transform.position)
            ).ToList();
            Dictionary<Platform, float> capacity = new();
            foreach (var platform in platforms) capacity[platform] = 1f;
            foreach (var enemyType in chosenEnemyTypes)
            {
                var maxPerPlatform = enemyType.maxPerPlatform == 0 ? 1 : enemyType.maxPerPlatform;
                var capacityToConsume = 1f / maxPerPlatform;
                var enemiesPerPlatform = totalPower / chosenEnemyTypes.Length / enemyType.power;
                var enemiesToSpawn = (int)Math.Floor(platforms.Count * enemiesPerPlatform);
                for (var i = 0; i < enemiesToSpawn; i++)
                {
                    var availablePlatforms =
                        platforms.Where(p => capacity[p] >= capacityToConsume);
                    var platform = Utils.RandomFromArray(availablePlatforms.ToArray());
                    capacity[platform] -= capacityToConsume;
                    var startingPosition = (Vector2)platform.transform.position + Vector2.up * 1f +
                                           Vector2.right * Random.Range(-1f, 1f);
                    Instantiate(enemyType.prefab, startingPosition, Quaternion.identity, transform);
                }
            }
        }

        private void SpawnUrns()
        {
            var platforms = FindObjectsOfType<Platform>().Where(p => !p.isReward).ToArray();
            var maxPerPlatform = 2;
            var totalUrns = platforms.Length / 2;
            Dictionary<Platform, int> platformToNumUrns = new();
            foreach (var platform in platforms) platformToNumUrns[platform] = 0;
            for (var i = 0; i < totalUrns; i++)
            {
                var platform = Utils.RandomFromArray(platforms
                    .Where(p => platformToNumUrns[p] < maxPerPlatform).ToArray());
                platformToNumUrns[platform]++;
            }

            foreach (var platform in platforms)
            {
                var numUrns = platformToNumUrns[platform];
                foreach (var offset in Utils.GetIntervalsAroundZero(numUrns, 1f))
                {
                    var startingPosition = (Vector2)platform.transform.position + Vector2.up +
                                           Vector2.right * offset;
                    Instantiate(urnPrefab, startingPosition, Quaternion.identity, transform);
                }
            }
        }

        public void GenerateLevel(int levelNum, int stageNum)
        {
            var stage = stages.stages[stageNum - 1];
            if (levelNum - 1 == stage.levels.Length)
            {
                Lava.Instance.SetTarget(_lastLocation.y);
                var bossLevel = Instantiate(stage.bossLevel.gameObject, _lastLocation + Vector2.up * 6f,
                    Quaternion.identity,
                    transform);
                _lastLocation = bossLevel.GetComponent<BossLevel>().rewardPlatform.transform.position;
                LevelManager.Instance.StopMusic();
            }
            else
            {
                Lava.Instance.ClearTarget();
                var level = stage.levels[levelNum - 1];
                var size = level.size == 0 ? 30 : level.size;
                for (var i = 0; i < size; i++)
                    GeneratePlatformLayer();
                var isFirstLevel = levelNum == 1 && stageNum == 1;
                GenerateRewardPlatform(isFirstLevel);
                if (!isFirstLevel)
                {
                    SpawnUrns();
                    SpawnEnemies(level.enemyPower, stage.enemies);
                }
            }
        }
    }
}