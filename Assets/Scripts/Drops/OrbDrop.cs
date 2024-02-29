using UnityEngine;

namespace Drops
{
    public class OrbDrop : MonoBehaviour
    {
        public AudioClip pickUpClip;

        private void Update()
        {
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) <= 1f &&
                Player.Instance.orbs < Player.Instance.maxOrbs)
            {
                Player.Instance.ChangeOrbs(1);
                Player.Instance.audioSource.PlayOneShot(pickUpClip);
                Destroy(gameObject);
            }
        }
    }
}