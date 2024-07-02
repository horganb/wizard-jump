namespace Drops
{
    public class GoldDrop : Drop
    {
        protected override bool CanPickUp()
        {
            return true;
        }

        protected override void OnPickUp()
        {
            Player.Instance.gold++;
        }
    }
}