using Interactable;

namespace RewardType
{
    public class AttackUpgrade : RewardType
    {
        public override IChestReward[] GenerateRewards()
        {
            Player.Instance.timesAttackUpgraded++;
            return Utils.InstantiateRandomSubclassXTimes<AttackUpgrades.AttackUpgrade>(2).ToArray();
        }

        public override bool CanSpawn()
        {
            return Player.Instance.ActiveAttack is not null;
        }

        public override int GetCost()
        {
            return 25 + Player.Instance.timesAttackUpgraded * 15;
        }

        public override string PurchasePrompt()
        {
            return "Upgrade spell";
        }
    }
}