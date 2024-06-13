using GamGUI;

namespace Gear
{
    public class Armor : Gear
    {
        public override string DisplayName()
        {
            return "Armor of Thorns";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.armor.gameObject.SetActive(true);
            Player.Instance.thorns += 2f;
            GameGUI.Instance.DisplayMessage("Retaliatory damage increased!");
        }

        protected override string SpriteName()
        {
            return "Armor";
        }
    }
}