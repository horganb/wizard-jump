using System;
using Enemies;
using UnityEngine;

namespace Projectiles
{
    public class Bomb : MonoBehaviour
    {
        public GameObject explosionPrefab;
        public float explosionRange = 3f;
        public float damage = 2f;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var fireballViewportPoint = _mainCamera.WorldToViewportPoint(gameObject.transform.position);
            if (fireballViewportPoint.x > 1.2 || Math.Abs(fireballViewportPoint.y) > 1.2) Destroy(gameObject);
        }

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<Enemy>() != null)
            {
                // apply damage to all enemies in range
                foreach (var otherEnemy in FindObjectsOfType<Enemy>())
                {
                    var impactVector = otherEnemy.transform.position - transform.position;
                    if (impactVector.magnitude <= explosionRange) otherEnemy.OnHit(impactVector.normalized, damage);
                }

                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}