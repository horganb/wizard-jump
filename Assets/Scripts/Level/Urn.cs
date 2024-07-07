using Enemies;
using UnityEngine;

namespace Level
{
    public class Urn : Hittable
    {
        public GameObject shatterPrefab;
        public GameObject silverPrefab;
        public GameObject goldPrefab;
        public GameObject diamondPrefab;
        public GameObject sapphirePrefab;
        public GameObject healthPrefab;
        private bool _destroyed;

        public override void OnHit(Vector2 impactVector, float damage, GameObject projectile = null)
        {
            if (_destroyed) return;
            _destroyed = true;
            var position = transform.position;
            Instantiate(shatterPrefab, position, Quaternion.identity);
            var randomValue = Random.value / (1 + Player.Instance.LootChanceIncrease());
            var prefabToInstantiate =
                randomValue <= 0.1f ? sapphirePrefab :
                randomValue <= 0.3f ? goldPrefab : silverPrefab;
            Instantiate(prefabToInstantiate, position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}