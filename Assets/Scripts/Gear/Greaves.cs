namespace Gear
{
    public class Greaves : Gear
    {
        public override string Name()
        {
            return "Greaves of Lava Resist";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.greaves.gameObject.SetActive(true);
            Player.Instance.lavaDamage -= 0.25f;
        }

        protected override string SpriteName()
        {
            return "Greaves";
        }
    }
}