using System.Collections.Generic;
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
                randomValue <= 0.1f ? sapphirePrefab : goldPrefab;
            var rewardObjects = prefabToInstantiate == goldPrefab && randomValue <= 0.3f
                ? new List<GameObject>
                {
                    Instantiate(prefabToInstantiate, position + Vector3.left * 0.4f,
                        Quaternion.identity),
                    Instantiate(prefabToInstantiate, position + Vector3.right * 0.4f,
                        Quaternion.identity)
                }
                : new List<GameObject>
                    { Instantiate(prefabToInstantiate, position, Quaternion.identity) };
            foreach (var rewardObject in rewardObjects)
                rewardObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
}