using Enemies;

namespace Projectiles
{
    public class Fireball : Projectile
    {
        protected override void OnHitTarget(Hittable target)
        {
            base.OnHitTarget(target);
            target.SetOnFire();
        }
    }
}