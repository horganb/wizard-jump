namespace AttackUpgrades
{
    public class AttackDamage : AttackUpgrade
    {
        public override string Name()
        {
            return "Increase Attack Damage";
        }

        public override void Acquire()
        {
            Player.Instance.damage += 1f;
        }
    }
}