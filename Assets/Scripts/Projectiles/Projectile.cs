using Enemies;
using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        public float speed = 2f;
        private Vector3 _direction;

        protected virtual void Start()
        {
            _direction = transform.right;
        }

        protected virtual void Update()
        {
            Utils.DestroyIfOffscreen(gameObject);
            transform.Translate(_direction * (Time.deltaTime * speed), Space.World);
        }

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            var enemy = col.gameObject.GetComponent<Enemy>();
            if (enemy != null) OnHitEnemy(enemy);
        }

        protected virtual void OnHitEnemy(Enemy enemy)
        {
            var impactVector = enemy.transform.position - transform.position;
            enemy.OnHit(impactVector, Player.Instance.damage);
            Destroy(gameObject);
        }
    }
}