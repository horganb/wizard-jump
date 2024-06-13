using UnityEngine;

namespace Projectiles
{
    public class Saw : Projectile
    {
        public float spinSpeed = 1000f;
        public GameObject destroyedEffect;
        private Rigidbody2D _rigidBody;

        protected override void Start()
        {
            base.Start();
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.angularVelocity = spinSpeed;
        }

        protected override void OnCollisionEnter2D(Collision2D col)
        {
            // do nothing
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            var platform = col.gameObject.GetComponent<Platform>();
            if (platform != null && !platform.isReward)
            {
                Instantiate(destroyedEffect, platform.transform.position, Quaternion.identity);
                Destroy(platform.gameObject);
                Destroy(gameObject);
            }
        }
    }
}