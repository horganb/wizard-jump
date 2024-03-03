using UnityEngine;

namespace Drops
{
    public class HealthDrop : MonoBehaviour
    {
        public AudioClip pickUpClip;
        public float speed = 2f;

        private void Update()
        {
            if (Player.Instance.health >= Player.Instance.maxHealth) return;

            var position = transform.position;
            var distanceFromPlayer = Vector2.Distance(position, Player.Instance.transform.position);
            if (distanceFromPlayer <= 5f)
                transform.position +=
                    (Player.Instance.transform.position - position).normalized * (speed * Time.deltaTime);

            if (distanceFromPlayer <= 0.5f)
            {
                Player.Instance.OnGainHealth(0.5f);
                Player.Instance.audioSource.PlayOneShot(pickUpClip);
                Destroy(gameObject);
            }
        }
    }
}