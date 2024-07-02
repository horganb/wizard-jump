using Enemies;
using UnityEngine;

namespace Level
{
    public class Urn : Hittable
    {
        public GameObject shatterPrefab;
        public GameObject goldPrefab;
        public GameObject sapphirePrefab;
        public GameObject healthPrefab;

        public override void OnHit(Vector2 impactVector, float damage, GameObject projectile = null)
        {
            var position = transform.position;
            Instantiate(shatterPrefab, position, Quaternion.identity);
            var randomValue = Random.value;
            var prefabToInstantiate =
                randomValue <= 0.1f ? healthPrefab : randomValue <= 0.2f ? sapphirePrefab : goldPrefab;
            Instantiate(prefabToInstantiate, position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}