using Interactable;

namespace RewardType
{
    public class GainAbility : RewardType
    {
        public override IChestReward[] GenerateRewards()
        {
            return Utils.InstantiateRandomSubclassXTimes<Special.Special>(2).ToArray();
        }

        public override bool CanSpawn()
        {
            return Player.Instance.Special is null;
        }

        public override int GetCost()
        {
            return 20;
        }

        public override string PurchasePrompt()
        {
            return "Buy special ability";
        }
    }
}