using System;
using System.Collections;
using GamGUI;
using UnityEngine;

namespace Singletons
{
    public class Lava : SingletonMonoBehaviour<Lava>
    {
        public float growRate = 0.5f;
        public float speedGrowRate = 50f;
        public bool isPaused;
        public bool isHalted;
        public AudioClip lavaDeathClip;

        private Camera _mainCamera;
        private bool _reachingTarget;
        private float _yTarget;

        // Start is called before the first frame update
        private void Start()
        {
            _mainCamera = Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            var position = transform.position;
            if (IsPaused()) return;
            if (_reachingTarget)
            {
                var underBy = _yTarget - position.y;
                if (Math.Abs(underBy) > 0.1f)
                {
                    var changeAmount = Time.deltaTime * speedGrowRate * (underBy > 0f ? 1f : -1f);
                    position.y += changeAmount;
                    transform.position = position;
                }

                return;
            }

            var bottomOfScreen = _mainCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).y - 1f;
            var lastPlayerPlatform = Player.Instance.lastStandingPlatform
                ? Player.Instance.lastStandingPlatform.transform.position.y - 1f
                : bottomOfScreen;
            var minY = Math.Min(lastPlayerPlatform, bottomOfScreen);
            position.y += Time.deltaTime * (position.y < minY ? speedGrowRate : growRate);

            transform.position = position;
        }

        public void SetTarget(float yTarget)
        {
            _yTarget = yTarget;
            _reachingTarget = true;
        }

        public void ClearTarget()
        {
            _reachingTarget = false;
        }

        private bool IsPaused()
        {
            return isPaused || isHalted;
        }

        public IEnumerator HaltLavaFor(float seconds)
        {
            GameGUI.Instance.DisplayMessage("Lava frozen!");
            isHalted = true;
            yield return new WaitForSeconds(seconds);
            GameGUI.Instance.DisplayMessage("Lava unfrozen!", GameGUI.MessageTone.Negative);
            isHalted = false;
        }
    }
}