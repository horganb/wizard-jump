using UnityEngine;

namespace Levels
{
    public abstract class BossLevel : MonoBehaviour
    {
        public GameObject rewardPlatform;

        public void OnBeaten()
        {
            rewardPlatform.SetActive(true);
        }
    }
}