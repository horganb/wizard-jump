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
        public bool isEnabled = true;
        public Collider2D theCollider;

        private void Start()
        {
            theCollider = GetComponent<Collider2D>();
            isEnabled = true;
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
            theCollider.excludeLayers = LayerMask.GetMask("Player");
            StartCoroutine(EndPassThroughAfterDelay());
        }

        private IEnumerator EndPassThroughAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            theCollider.excludeLayers = new LayerMask();
        }

        public void StartTempDestroy(float seconds)
        {
            StartCoroutine(TempDestroy(seconds));
        }

        private IEnumerator TempDestroy(float seconds)
        {
            var colliders = GetComponentsInChildren<Collider2D>();
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (var comp in colliders) comp.enabled = false;
            foreach (var comp in renderers) comp.enabled = false;
            isEnabled = false;
            yield return new WaitForSeconds(seconds);
            foreach (var comp in colliders) comp.enabled = true;
            foreach (var comp in renderers) comp.enabled = true;
            isEnabled = true;
        }
    }
}