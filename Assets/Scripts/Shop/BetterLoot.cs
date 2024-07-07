namespace Shop
{
    public class BetterLoot : ShopItem
    {
        public override string Name()
        {
            return "+ Rare Loot Chance";
        }

        public override int Price()
        {
            return 60 + 30 * Player.Instance.timesBetterLootBought;
        }

        public override void OnPurchase()
        {
            Player.Instance.timesBetterLootBought++;
        }
    }
}