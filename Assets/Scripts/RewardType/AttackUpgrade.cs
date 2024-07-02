using Interactable;

namespace RewardType
{
    public class AttackUpgrade : RewardType
    {
        public override IChestReward[] GenerateRewards()
        {
            return Utils.InstantiateRandomSubclassXTimes<AttackUpgrades.AttackUpgrade>(2).ToArray();
        }

        public override bool CanSpawn()
        {
            return Player.Instance.ActiveAttack is not null;
        }

        public override int GetCost()
        {
            return 20;
        }

        public override string PurchasePrompt()
        {
            return "Upgrade spell";
        }
    }
}