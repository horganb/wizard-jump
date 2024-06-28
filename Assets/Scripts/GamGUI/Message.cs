using UnityEngine;

namespace GamGUI
{
    public class Message : MonoBehaviour
    {
        private void Update()
        {
            if (transform.lossyScale.x < 0f)
            {
                var transform1 = transform;
                var localScale = transform1.localScale;
                localScale.x *= -1;
                transform1.localScale = localScale;
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}