using Enemies;
using UnityEngine;

namespace Projectiles
{
    public class Bomb : Projectile
    {
        public GameObject explosionPrefab;
        public float explosionRange = 3f;
        public float damage = 2f;

        private void Update()
        {
            Utils.DestroyIfOffscreen(gameObject);
        }

        protected override void OnHitEnemy(Enemy enemy)
        {
            // apply damage to all enemies in range
            foreach (var otherEnemy in FindObjectsOfType<Enemy>())
            {
                var impactVector = otherEnemy.transform.position - transform.position;
                if (impactVector.magnitude <= explosionRange) otherEnemy.OnHit(impactVector.normalized, damage);
            }

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}