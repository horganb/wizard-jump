using Enemies;
using UnityEngine;

namespace Level
{
    public class Urn : Hittable
    {
        public GameObject shatterPrefab;

        public override void OnHit(Vector2 impactVector, float damage)
        {
            Instantiate(shatterPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}