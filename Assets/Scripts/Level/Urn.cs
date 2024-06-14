using Enemies;
using UnityEngine;

namespace Level
{
    public class Urn : Hittable
    {
        public GameObject shatterPrefab;
        public GameObject sapphirePrefab;

        public override void OnHit(Vector2 impactVector, float damage, GameObject projectile = null)
        {
            var position = transform.position;
            Instantiate(shatterPrefab, position, Quaternion.identity);
            Instantiate(sapphirePrefab, position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}