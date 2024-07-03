using Interactable;

namespace RewardType
{
    public class UpgradeGear : RewardType
    {
        public override IChestReward[] GenerateRewards()
        {
            return Utils.InstantiateRandomSubclassXTimes<Gear.Gear>(2).ToArray();
        }

        public override bool CanSpawn()
        {
            return true;
        }

        public override int GetCost()
        {
            return 10;
        }

        public override string PurchasePrompt()
        {
            return "Buy gear";
        }
    }
}