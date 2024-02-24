using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamGUI
{
    public class ManaIndicator : MonoBehaviour
    {
        public GameObject manaBallPrefab;
        public Sprite fullSprite;
        public Sprite emptySprite;
        private List<Image> _manaBallImages;

        private Player _player;
        private int _prevMana;

        // Start is called before the first frame update
        private void Start()
        {
            _manaBallImages = new List<Image>();
            _player = FindObjectOfType<Player>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.childCount != _player.maxMana)
            {
                foreach (var manaBallImage in _manaBallImages) Destroy(manaBallImage.gameObject);
                _manaBallImages.Clear();
                for (var i = 0; i < _player.maxMana; i++)
                {
                    var manaBall = Instantiate(manaBallPrefab, Vector2.zero, Quaternion.identity, transform);
                    _manaBallImages.Add(manaBall.GetComponent<Image>());
                }
            }

            var playerManaTruncated = (int)Math.Floor(_player.mana);
            if (_prevMana != playerManaTruncated)
            {
                _prevMana = playerManaTruncated;
                for (var i = 0; i < _player.maxMana; i++)
                    _manaBallImages[i].sprite = playerManaTruncated > i ? fullSprite : emptySprite;
            }
        }
    }
}