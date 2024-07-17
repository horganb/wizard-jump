using Enemies;
using UnityEngine;

namespace Level
{
    public class SlimeResidue : Hittable
    {
        public float lifespan = 2f;
        public GameObject destroyedPrefab;
        private float _aliveTimer;

        private void Update()
        {
            _aliveTimer += Time.deltaTime;
            if (_aliveTimer >= lifespan) Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var player = col.gameObject.GetComponent<Player>();
            if (player != null && !player.HasInvincibility())
            {
                player.OnStuck();
                Destroy(gameObject);
            }
        }

        public override void OnHit(Vector2 impactVector, float damage, GameObject projectile = null)
        {
            Destroy(gameObject);
            Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
        }
    }
}