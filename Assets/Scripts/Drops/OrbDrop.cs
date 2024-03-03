using UnityEngine;

namespace Drops
{
    public class OrbDrop : MonoBehaviour
    {
        public AudioClip pickUpClip;
        public float speed = 2f;

        private void Update()
        {
            if (Player.Instance.orbs >= Player.Instance.maxOrbs) return;

            var position = transform.position;
            var distanceFromPlayer = Vector2.Distance(position, Player.Instance.transform.position);
            if (distanceFromPlayer <= 5f)
                transform.position +=
                    (Player.Instance.transform.position - position).normalized * (speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, Player.Instance.transform.position) <= 1f)
            {
                Player.Instance.ChangeOrbs(1);
                Player.Instance.audioSource.PlayOneShot(pickUpClip);
                Destroy(gameObject);
            }
        }
    }
}