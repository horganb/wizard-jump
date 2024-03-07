namespace Gear
{
    public class Armor : Gear
    {
        public override string Name()
        {
            return "Armor of Thorns";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.armor.gameObject.SetActive(true);
            Player.Instance.thorns += 1f;
        }

        protected override string SpriteName()
        {
            return "Armor";
        }
    }
}