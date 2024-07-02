using System;
using UnityEngine;

namespace Drops
{
    public abstract class Drop : MonoBehaviour
    {
        public AudioClip pickUpClip;
        public bool shouldMoveTowardsPlayer = true;
        public float distanceThreshold = 0.5f;

        private void Update()
        {
            if (ShouldDestroy())
            {
                Destroy(gameObject);
                return;
            }

            var position = transform.position;
            var playerPosition = Player.Instance.transform.position;
            var distanceFromPlayer = Vector2.Distance(position, playerPosition);
            if (shouldMoveTowardsPlayer)
            {
                var realSpeed = Math.Clamp(10f / distanceFromPlayer, 0.5f, 4f);
                position += (playerPosition - position).normalized * (realSpeed * Time.deltaTime);
                transform.position = position;
            }

            if (distanceFromPlayer <= distanceThreshold && CanPickUp())
            {
                OnPickUp();
                Player.Instance.audioSource.PlayOneShot(pickUpClip);
                Destroy(gameObject);
            }
        }

        protected virtual bool ShouldDestroy()
        {
            return false;
        }

        protected abstract bool CanPickUp();

        protected abstract void OnPickUp();
    }
}