namespace Gear
{
    public class Goggles : Gear
    {
        public override string Name()
        {
            return "Scavenging Goggles";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.goggles.gameObject.SetActive(true);
            Player.Instance.dropModifier += 0.1f;
        }

        protected override string SpriteName()
        {
            return "Goggles";
        }
    }
}