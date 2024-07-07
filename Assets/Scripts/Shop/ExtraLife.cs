namespace Shop
{
    public class ExtraLife : ShopItem
    {
        public override string Name()
        {
            return "+1 Health";
        }

        public override int Price()
        {
            return 100 + 50 * Player.Instance.timesExtraLifeBought;
        }

        public override void OnPurchase()
        {
            Player.Instance.timesExtraLifeBought++;
            Player.Instance.health = Player.Instance.MaxHealth();
        }
    }
}