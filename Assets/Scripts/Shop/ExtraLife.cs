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
            return 20 + 20 * Player.Instance.timesExtraLifeBought;
        }

        public override void OnPurchase()
        {
            Player.Instance.maxHealth++;
            Player.Instance.health = Player.Instance.maxHealth;
            Player.Instance.timesExtraLifeBought++;
        }
    }
}