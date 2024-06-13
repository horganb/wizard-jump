using Enemies;

namespace Projectiles
{
    public class IceSpike : Projectile
    {
        protected override void OnHitEnemy(Enemy enemy)
        {
            base.OnHitEnemy(enemy);
            enemy.Freeze();
        }
    }
}