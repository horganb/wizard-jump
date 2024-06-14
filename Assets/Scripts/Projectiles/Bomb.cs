using Enemies;
using UnityEngine;

namespace Projectiles
{
    public class Bomb : Projectile
    {
        public GameObject explosionPrefab;
        public float explosionRange = 3f;
        public float damage = 2f;

        protected override void Update()
        {
            Utils.DestroyIfOffscreen(gameObject);
        }

        protected override void OnHitTarget(Hittable target)
        {
            // apply damage to all targets in range
            foreach (var otherTarget in FindObjectsOfType<Hittable>())
            {
                var impactVector = otherTarget.transform.position - transform.position;
                if (impactVector.magnitude <= explosionRange) otherTarget.OnHit(impactVector.normalized, damage);
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}