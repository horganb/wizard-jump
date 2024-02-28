using System.Collections.Generic;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace GamGUI
{
    public class LivesIndicator : SingletonMonoBehaviour<LivesIndicator>
    {
        public GameObject heartPrefab;
        public Sprite fullSprite;
        public Sprite emptySprite;
        private List<Image> _heartImages;

        private int _prevLives;

        // Start is called before the first frame update
        private void Start()
        {
            _heartImages = new List<Image>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.childCount != Player.Instance.maxLives)
            {
                foreach (var heartImage in _heartImages) Destroy(heartImage.gameObject);
                _heartImages.Clear();
                for (var i = 0; i < Player.Instance.maxLives; i++)
                {
                    var heart = Instantiate(heartPrefab, Vector2.zero, Quaternion.identity, transform);
                    _heartImages.Add(heart.GetComponent<Image>());
                }
            }

            if (_prevLives != Player.Instance.lives)
            {
                _prevLives = Player.Instance.lives;
                for (var i = 0; i < Player.Instance.maxLives; i++)
                    _heartImages[i].sprite = Player.Instance.lives > i ? fullSprite : emptySprite;
            }
        }
    }
}