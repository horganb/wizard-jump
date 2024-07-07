using Save;

namespace Drops
{
    public class SapphireDrop : Drop
    {
        protected override bool CanPickUp()
        {
            return true;
        }

        protected override void OnPickUp()
        {
            Player.Instance.sapphire++;
            SaveManager.Save();
        }
    }
}