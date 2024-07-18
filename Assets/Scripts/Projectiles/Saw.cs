using Level;
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

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            var platform = col.gameObject.GetComponent<Platform>();
            if (platform != null && !platform.isReward)
            {
                Instantiate(destroyedEffect, platform.transform.position, Quaternion.identity);
                platform.StartTempDestroy(1f);
                Destroy(gameObject);
            }
        }
    }
}