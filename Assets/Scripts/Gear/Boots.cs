namespace Gear
{
    public class Boots : Gear
    {
        public override string Name()
        {
            return "Boots of Escape";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.boots.gameObject.SetActive(true);
            Player.Instance.dodgeChance += 0.2f;
        }

        protected override string SpriteName()
        {
            return "Boots";
        }
    }
}