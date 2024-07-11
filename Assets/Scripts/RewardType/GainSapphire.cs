using Interactable;
using Save;
using UnityEngine;

namespace RewardType
{
    public class GainSapphire : RewardType, IChestReward
    {
        public Sprite sapphire;

        public string Name()
        {
            return "15 Sapphire";
        }

        public void Acquire()
        {
            Player.Instance.sapphire += 15;
            SaveManager.Save();
        }

        public Sprite GetSprite()
        {
            return sapphire;
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
            return 30;
        }

        public override string PurchasePrompt()
        {
            return "Buy 15 sapphire";
        }
    }
}