using System;
using System.Collections.Generic;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace GamGUI
{
    public class ManaIndicator : SingletonMonoBehaviour<ManaIndicator>
    {
        public GameObject manaBallPrefab;
        public Sprite fullSprite;
        public Sprite emptySprite;
        private List<Image> _manaBallImages;

        private int _prevMana;

        // Start is called before the first frame update
        private void Start()
        {
            _manaBallImages = new List<Image>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.childCount != Player.Instance.maxMana)
            {
                foreach (var manaBallImage in _manaBallImages) Destroy(manaBallImage.gameObject);
                _manaBallImages.Clear();
                for (var i = 0; i < Player.Instance.maxMana; i++)
                {
                    var manaBall = Instantiate(manaBallPrefab, Vector2.zero, Quaternion.identity, transform);
                    _manaBallImages.Add(manaBall.GetComponent<Image>());
                }
            }

            var playerManaTruncated = (int)Math.Floor(Player.Instance.mana);
            if (_prevMana != playerManaTruncated)
            {
                _prevMana = playerManaTruncated;
                for (var i = 0; i < Player.Instance.maxMana; i++)
                    _manaBallImages[i].sprite = playerManaTruncated > i ? fullSprite : emptySprite;
            }
        }
    }
}