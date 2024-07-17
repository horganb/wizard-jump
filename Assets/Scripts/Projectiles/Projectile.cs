using Enemies;
using UnityEngine;

namespace Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        public GameObject alertPrefab;
        public float speed = 2f;
        private GameObject _alert;

        protected virtual void Update()
        {
            Utils.DestroyIfOffscreen(gameObject);
            transform.Translate(transform.right * (Time.deltaTime * speed), Space.World);
            if (_alert is not null) _alert.transform.position = transform.position + Vector3.up * 0.5f;
        }

        private void OnDestroy()
        {
            Destroy(_alert);
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            var target = col.gameObject.GetComponent<Hittable>();
            if (target != null) OnHitTarget(target);

            var player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Player.Instance.OnHit(0.25f, gameObject);
                Destroy(gameObject);
            }
        }

        protected virtual void OnHitTarget(Hittable target)
        {
            var impactVector = target.transform.position - transform.position;
            target.OnHit(impactVector, Player.Instance.damage, gameObject);
        }

        public void SetCanDamagePlayer()
        {
            _alert = Instantiate(alertPrefab);
            GetComponent<Collider2D>().includeLayers = LayerMask.GetMask("Player");
        }
    }
}