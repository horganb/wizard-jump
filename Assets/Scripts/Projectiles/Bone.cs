using UnityEngine;

namespace Projectiles
{
    public class Bone : MonoBehaviour
    {
        public float damage = 0.5f;

        protected void Update()
        {
            Utils.DestroyIfBelowScreen(gameObject);
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            var player = col.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.OnHit(damage, gameObject);
                Destroy(gameObject);
            }
        }
    }
}