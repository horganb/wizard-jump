﻿using Enemies;
using Level;
using UnityEngine;

namespace Projectiles
{
    public class SlimeBlob : Hittable
    {
        public float damage = 0.25f;
        public GameObject residuePrefab;
        public GameObject destroyedPrefab;
        private Rigidbody2D _rigidbody;

        protected void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        protected void Update()
        {
            Utils.DestroyIfBelowScreen(gameObject);
        }

        protected override void OnTriggerEnter2D(Collider2D col)
        {
            base.OnTriggerEnter2D(col);
            var player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.OnHit(damage, gameObject);
                Destroy(gameObject);
                return;
            }

            var platform = col.gameObject.GetComponent<Platform>();
            if (platform != null)
            {
                var position = transform.position;
                var playerPosition = Player.Instance.transform.position;
                if ((_rigidbody.velocity.x > 0f && position.x > playerPosition.x) ||
                    (_rigidbody.velocity.x < 0f && position.x < playerPosition.x) ||
                    (_rigidbody.velocity.y < 0f && position.y < playerPosition.y))
                {
                    Instantiate(residuePrefab, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }

        public override void OnHit(Vector2 impactVector, float damage, GameObject projectile = null)
        {
            Destroy(gameObject);
            Instantiate(destroyedPrefab, transform.position, Quaternion.identity);
        }
    }
}