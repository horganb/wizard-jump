using Interactable;
using UnityEngine;

namespace RewardType
{
    public class HealthReward : RewardType, IChestReward
    {
        public Sprite heartSprite;

        public string Name()
        {
            return "Health";
        }

        public void Acquire()
        {
            Player.Instance.OnGainHealth(1f);
        }

        public Sprite GetSprite()
        {
            return heartSprite;
        }

        public override IChestReward[] GenerateRewards()
        {
            return new IChestReward[] { this };
        }

        public override bool CanSpawn()
        {
            return true;
        }

        public override int GetCost()
        {
            return 5;
        }

        public override string PurchasePrompt()
        {
            return "Buy 1 health";
        }
    }
}