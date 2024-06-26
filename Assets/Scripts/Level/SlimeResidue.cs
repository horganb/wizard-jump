using Enemies;
using UnityEngine;

namespace Level
{
    public class SlimeResidue : Hittable
    {
        public float lifespan = 2f;
        private float _aliveTimer;

        private void Update()
        {
            _aliveTimer += Time.deltaTime;
            if (_aliveTimer >= lifespan) Destroy(gameObject);
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            base.OnCollisionEnter2D(col);
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
        }
    }
}