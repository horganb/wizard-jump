using System;
using System.Collections.Generic;
using Singletons;
using UnityEngine;

namespace GamGUI
{
    public class HealthIndicator : SingletonMonoBehaviour<HealthIndicator>
    {
        public GameObject heartPrefab;
        private List<HeartIndicator> _heartImages;

        // Start is called before the first frame update
        private void Start()
        {
            _heartImages = new List<HeartIndicator>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.childCount != (int)Math.Ceiling(Player.Instance.MaxHealth()))
            {
                foreach (var heartImage in _heartImages) Destroy(heartImage.gameObject);
                _heartImages.Clear();
                for (var i = 0; i < Player.Instance.MaxHealth(); i++)
                {
                    var heart = Instantiate(heartPrefab, Vector2.zero, Quaternion.identity, transform);
                    _heartImages.Add(heart.GetComponent<HeartIndicator>());
                }
            }

            for (var i = 0; i < Player.Instance.MaxHealth(); i++)
            {
                var heartFullness = Math.Clamp(Player.Instance.health - i, 0f, 1f);
                var fullnessRect = _heartImages[i].fullnessRect;
                fullnessRect.sizeDelta =
                    new Vector2(fullnessRect.sizeDelta.x, heartFullness * 100f);
            }
        }
    }
}