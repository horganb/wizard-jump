using UnityEngine;

namespace Drops
{
    public class HealthDrop : MonoBehaviour
    {
        public AudioClip pickUpClip;

        private void Update()
        {
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) <= 1f &&
                Player.Instance.health < Player.Instance.maxHealth)
            {
                Player.Instance.OnGainHealth(0.5f);
                Player.Instance.audioSource.PlayOneShot(pickUpClip);
                Destroy(gameObject);
            }
        }
    }
}