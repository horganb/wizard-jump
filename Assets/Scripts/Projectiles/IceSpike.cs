using Enemies;

namespace Projectiles
{
    public class IceSpike : Projectile
    {
        protected override void OnHitTarget(Hittable target)
        {
            base.OnHitTarget(target);
            target.Freeze();
        }
    }
}