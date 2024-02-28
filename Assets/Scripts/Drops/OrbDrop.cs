using UnityEngine;

namespace Drops
{
    public class OrbDrop : MonoBehaviour
    {
        private void Update()
        {
            if (Vector2.Distance(transform.position, Player.Instance.transform.position) <= 1f &&
                Player.Instance.orbs < Player.Instance.maxOrbs)
            {
                Player.Instance.ChangeOrbs(1);
                Destroy(gameObject);
            }
        }
    }
}