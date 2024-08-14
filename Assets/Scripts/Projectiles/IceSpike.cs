using Enemies;
using UnityEngine;

namespace Projectiles
{
    public class IceSpike : Projectile
    {
        protected override void OnHitTarget(Hittable target)
        {
            base.OnHitTarget(target);
            if (Random.value > 0.7f) target.Freeze();
        }
    }
}