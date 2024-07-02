namespace AttackUpgrades
{
    public class AttackSpeed : AttackUpgrade
    {
        public override string Name()
        {
            return "Increase Attack Speed";
        }

        public override void Acquire()
        {
            Player.Instance.attackSpeed -= 0.1f;
        }
    }
}