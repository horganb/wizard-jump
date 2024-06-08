using System;
using UnityEngine;

namespace Drops
{
    public abstract class Drop : MonoBehaviour
    {
        public AudioClip pickUpClip;

        private void Update()
        {
            if (!CanPickUp())
            {
                Destroy(gameObject);
                return;
            }

            var position = transform.position;
            var playerPosition = Player.Instance.transform.position;
            var distanceFromPlayer = Vector2.Distance(position, playerPosition);
            var realSpeed = Math.Clamp(10f / distanceFromPlayer, 0.5f, 4f);
            transform.position += (playerPosition - position).normalized * (realSpeed * Time.deltaTime);

            if (distanceFromPlayer <= 0.5f)
            {
                OnPickUp();
                Player.Instance.audioSource.PlayOneShot(pickUpClip);
                Destroy(gameObject);
            }
        }

        protected abstract bool CanPickUp();

        protected abstract void OnPickUp();
    }
}