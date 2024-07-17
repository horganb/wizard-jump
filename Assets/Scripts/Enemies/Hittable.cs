using Singletons;
using UnityEngine;

namespace Enemies
{
    public abstract class Hittable : MonoBehaviour
    {
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            var lava = col.gameObject.GetComponent<Lava>();
            if (lava != null)
            {
                AudioSource.PlayClipAtPoint(lava.lavaDeathClip, transform.position);
                Destroy(gameObject);
            }
        }

        public abstract void OnHit(Vector2 impactVector, float damage, GameObject projectile = null);

        public virtual void SetOnFire()
        {
        }

        public virtual void Freeze()
        {
        }
    }
}