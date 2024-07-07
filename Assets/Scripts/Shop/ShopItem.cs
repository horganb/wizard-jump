namespace Shop
{
    public abstract class ShopItem
    {
        public abstract string Name();
        public abstract int Price();
        public abstract void OnPurchase();
    }
}