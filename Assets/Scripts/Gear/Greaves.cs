using GamGUI;

namespace Gear
{
    public class Greaves : Gear
    {
        public override string DisplayName()
        {
            return "Greaves of Lava Resist";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.greaves.gameObject.SetActive(true);
            Player.Instance.lavaDamage -= 0.25f;
            GameGUI.Instance.DisplayMessage("Lava damage decreased!");
        }

        protected override string SpriteName()
        {
            return "Greaves";
        }
    }
}