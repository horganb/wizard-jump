namespace AttackUpgrades
{
    public class AddProjectile : AttackUpgrade
    {
        public override string Name()
        {
            return "Add Projectile";
        }

        public override void Acquire()
        {
            Player.Instance.projectiles++;
        }
    }
}