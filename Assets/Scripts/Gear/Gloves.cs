namespace Gear
{
    public class Gloves : Gear
    {
        public override string Name()
        {
            return "Gloves of Quick Attack";
        }

        public override void Acquire()
        {
            PlayerGear.Instance.gloves.gameObject.SetActive(true);
            Player.Instance.attackSpeed -= 0.2f;
        }

        protected override string SpriteName()
        {
            return "Gloves";
        }
    }
}