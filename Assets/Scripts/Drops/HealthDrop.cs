namespace Drops
{
    public class HealthDrop : Drop
    {
        protected override bool CanPickUp()
        {
            return Player.Instance.health < Player.Instance.MaxHealth();
        }

        protected override void OnPickUp()
        {
            Player.Instance.OnGainHealth(0.5f);
        }
    }
}