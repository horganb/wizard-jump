using System;
using System.Collections;
using UnityEngine;

namespace Singletons
{
    public class Lava : SingletonMonoBehaviour<Lava>
    {
        public float growRate = 0.5f;
        public float speedGrowRate = 5f;
        public bool isPaused;
        public bool isHalted;
        public AudioClip lavaDeathClip;

        private Camera _mainCamera;

        // Start is called before the first frame update
        private void Start()
        {
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            if (IsPaused()) return;

            var obj = gameObject;
            var position = obj.transform.position;

            var bottomOfScreen = _mainCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).y - 1f;
            var lastPlayerPlatform = Player.Instance.lastStandingPlatform
                ? Player.Instance.lastStandingPlatform.transform.position.y - 1f
                : bottomOfScreen;
            var minY = Math.Min(lastPlayerPlatform, bottomOfScreen);
            position.y += Time.deltaTime * (position.y < minY ? speedGrowRate : growRate);

            obj.transform.position = position;
        }

        private bool IsPaused()
        {
            return isPaused || isHalted;
        }

        public IEnumerator HaltLavaFor(float seconds)
        {
            isHalted = true;
            yield return new WaitForSeconds(seconds);
            isHalted = false;
        }
    }
}