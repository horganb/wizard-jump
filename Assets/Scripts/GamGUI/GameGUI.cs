using Singletons;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace GamGUI
{
    public class GameGUI : SingletonMonoBehaviour<GameGUI>
    {
        public InteractionPrompt interactionPrompt;
        public GameObject gameOverScreen;
        public Image basicAttackImage;
        public Image specialImage;
        public Image potionImage;
        public Image scrollImage;
        public SpriteLibrary spriteLibrary;

        private void Update()
        {
            if (Player.Instance.Special != null) specialImage.sprite = Player.Instance.Special.GetSprite();
            specialImage.enabled = Player.Instance.Special != null;

            if (Player.Instance.Scroll != null) scrollImage.sprite = Player.Instance.Scroll.GetSprite();
            scrollImage.enabled = Player.Instance.Scroll != null;
        }
    }
}