using GamGUI;

namespace Gear
{
    public class Boots : Gear
    {
        public override string DisplayName()
        {
            return "Boots of Escape";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.boots.gameObject.SetActive(true);
            Player.Instance.dodgeChance += 0.2f;
            GameGUI.Instance.DisplayMessage("Dodge chance increased!");
        }

        protected override string SpriteName()
        {
            return "Boots";
        }
    }
}