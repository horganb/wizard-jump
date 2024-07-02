using System.Collections;
using UnityEngine;

namespace Level
{
    public class Platform : MonoBehaviour
    {
        public GameObject leftPlatform;
        public GameObject middlePlatform;
        public GameObject rightPlatform;
        public bool isReward;
        private Collider2D _collider;

        private void Start()
        {
            _collider = GetComponent<Collider2D>();
        }

        public void SetWidth(float middlePlatformWidth)
        {
            var spriteRenderer = middlePlatform.GetComponent<SpriteRenderer>();
            var size = spriteRenderer.size;
            size.x = middlePlatformWidth;
            spriteRenderer.size = size;

            var sidePlatformOffset = middlePlatformWidth / 2f;
            var middlePlatformPosition = middlePlatform.transform.position;

            var leftPlatformPosition = leftPlatform.transform.position;
            leftPlatformPosition.x = middlePlatformPosition.x - sidePlatformOffset;
            leftPlatform.transform.position = leftPlatformPosition;

            var rightPlatformPosition = rightPlatform.transform.position;
            rightPlatformPosition.x = middlePlatformPosition.x + sidePlatformOffset;
            rightPlatform.transform.position = rightPlatformPosition;
        }

        public void PlayerPassThrough()
        {
            _collider.excludeLayers = LayerMask.GetMask("Player");
            StartCoroutine(EndPassThroughAfterDelay());
        }

        private IEnumerator EndPassThroughAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _collider.excludeLayers = new LayerMask();
        }
    }
}