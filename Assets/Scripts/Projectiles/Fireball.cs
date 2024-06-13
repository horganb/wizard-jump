using Enemies;

namespace Projectiles
{
    public class Fireball : Projectile
    {
        protected override void OnHitEnemy(Enemy enemy)
        {
            base.OnHitEnemy(enemy);
            enemy.SetOnFire();
        }
    }
}