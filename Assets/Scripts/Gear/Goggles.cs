using GamGUI;

namespace Gear
{
    public class Goggles : Gear
    {
        public override string DisplayName()
        {
            return "Scavenging Goggles";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.goggles.gameObject.SetActive(true);
            Player.Instance.dropModifier += 0.1f;
            GameGUI.Instance.DisplayMessage("Loot increased!");
        }

        protected override string SpriteName()
        {
            return "Goggles";
        }
    }
}