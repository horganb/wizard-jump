namespace Drops
{
    public class GoldDrop : Drop
    {
        public int goldValue;

        protected override bool CanPickUp()
        {
            return true;
        }

        protected override void OnPickUp()
        {
            Player.Instance.gold += goldValue;
        }
    }
}