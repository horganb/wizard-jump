using Interactable;
using Scrolls;

namespace RewardType
{
    public class ScrollReward : RewardType
    {
        public override IChestReward[] GenerateRewards()
        {
            return new IChestReward[] { Utils.InstantiateRandomSubclass<Scroll>() };
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
            return "Buy scroll";
        }
    }
}