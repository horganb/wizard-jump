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
            return 15 + 15 * Player.Instance.timesBetterLootBought;
        }

        public override void OnPurchase()
        {
            Player.Instance.lootChanceIncrease += 0.1f;
            Player.Instance.timesBetterLootBought++;
        }
    }
}