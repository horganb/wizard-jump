using Attacks;
using Interactable;

namespace RewardType
{
    public class LearnSpell : RewardType
    {
        public override IChestReward[] GenerateRewards()
        {
            return Utils.InstantiateRandomSubclassXTimes<Attack>(2).ToArray();
        }

        public override bool CanSpawn()
        {
            return Player.Instance.ActiveAttack is null;
        }

        public override int GetCost()
        {
            return 0;
        }

        public override string PurchasePrompt()
        {
            return "Learn spell";
        }
    }
}