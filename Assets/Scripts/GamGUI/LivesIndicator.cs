using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GamGUI
{
    public class LivesIndicator : MonoBehaviour
    {
        public GameObject heartPrefab;
        public Sprite fullSprite;
        public Sprite emptySprite;
        private List<Image> _heartImages;

        private Player _player;
        private int _prevLives;

        // Start is called before the first frame update
        private void Start()
        {
            _heartImages = new List<Image>();
            _player = FindObjectOfType<Player>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.childCount != _player.maxLives)
            {
                foreach (var heartImage in _heartImages) Destroy(heartImage.gameObject);
                _heartImages.Clear();
                for (var i = 0; i < _player.maxLives; i++)
                {
                    var heart = Instantiate(heartPrefab, Vector2.zero, Quaternion.identity, transform);
                    _heartImages.Add(heart.GetComponent<Image>());
                }
            }

            if (_prevLives != _player.lives)
            {
                _prevLives = _player.lives;
                for (var i = 0; i < _player.maxLives; i++)
                    _heartImages[i].sprite = _player.lives > i ? fullSprite : emptySprite;
            }
        }
    }
}