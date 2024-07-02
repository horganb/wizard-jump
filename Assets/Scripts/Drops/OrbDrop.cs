namespace Drops
{
    public class OrbDrop : Drop
    {
        protected override bool ShouldDestroy()
        {
            return !CanPickUp();
        }

        protected override bool CanPickUp()
        {
            return Player.Instance.Special != null && Player.Instance.orbs < Player.Instance.maxOrbs;
        }

        protected override void OnPickUp()
        {
            Player.Instance.ChangeOrbs(1);
        }
    }
}