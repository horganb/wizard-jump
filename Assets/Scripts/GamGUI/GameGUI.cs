using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace GamGUI
{
    public class GameGUI : MonoBehaviour
    {
        public InteractionPrompt interactionPrompt;
        public GameObject gameOverScreen;
        public GameObject wandControl;
        public GameObject scrollControl;
        public Image basicAttackImage;
        public Image specialImage;
        public Image potionImage;
        public Image scrollImage;
        public SpriteLibrary spriteLibrary;

        private Player _player;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }

        private void Update()
        {
            if (_player.Special != null) specialImage.sprite = _player.Special.GetSprite();
            if (_player.Scroll != null) scrollImage.sprite = _player.Scroll.GetSprite();
            // wandControl.SetActive(_player.Special != null);
            // scrollControl.SetActive(_player.Scroll != null);
        }
    }
}