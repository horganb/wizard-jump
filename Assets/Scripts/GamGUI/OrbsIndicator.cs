using System.Collections.Generic;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace GamGUI
{
    public class OrbsIndicator : SingletonMonoBehaviour<OrbsIndicator>
    {
        public GameObject manaBallPrefab;
        public Sprite fullSprite;
        public Sprite emptySprite;
        private List<Image> _manaBallImages;

        // Start is called before the first frame update
        private void Start()
        {
            _manaBallImages = new List<Image>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (transform.childCount != Player.Instance.maxOrbs)
            {
                foreach (var manaBallImage in _manaBallImages) Destroy(manaBallImage.gameObject);
                _manaBallImages.Clear();
                for (var i = 0; i < Player.Instance.maxOrbs; i++)
                {
                    var manaBall = Instantiate(manaBallPrefab, Vector2.zero, Quaternion.identity, transform);
                    _manaBallImages.Add(manaBall.GetComponent<Image>());
                }
            }

            for (var i = 0; i < Player.Instance.maxOrbs; i++)
                _manaBallImages[i].sprite = Player.Instance.orbs > i ? fullSprite : emptySprite;
        }
    }
}