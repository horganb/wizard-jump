using GamGUI;

namespace Gear
{
    public class Gloves : Gear
    {
        public override string DisplayName()
        {
            return "Gloves of Quick Attack";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.gloves.gameObject.SetActive(true);
            Player.Instance.attackSpeed -= 0.1f;
            GameGUI.Instance.DisplayMessage("Attack speed increased!");
        }

        protected override string SpriteName()
        {
            return "Gloves";
        }
    }
}