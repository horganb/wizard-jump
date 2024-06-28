using UnityEngine;

namespace Level
{
    public class Platform : MonoBehaviour
    {
        public GameObject leftPlatform;
        public GameObject middlePlatform;
        public GameObject rightPlatform;
        public bool isReward;

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
    }
}