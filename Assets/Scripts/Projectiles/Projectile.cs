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

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            var target = col.gameObject.GetComponent<Hittable>();
            if (target != null) OnHitTarget(target);
        }

        protected virtual void OnHitTarget(Hittable target)
        {
            var impactVector = target.transform.position - transform.position;
            target.OnHit(impactVector, Player.Instance.damage);
            Destroy(gameObject);
        }
    }
}