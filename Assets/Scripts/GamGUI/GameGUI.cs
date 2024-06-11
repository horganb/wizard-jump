using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace GamGUI
{
    public class GameGUI : SingletonMonoBehaviour<GameGUI>
    {
        public enum MessageTone
        {
            Neutral,
            Positive,
            Negative
        }

        public InteractionPrompt interactionPrompt;
        public ChoiceInteractionPrompt choiceInteractionPrompt;
        public GameObject gameOverScreen;
        public GameObject basicAttack;
        public Image basicAttackImage;
        public GameObject special;
        public Image specialImage;
        public Image potionImage;
        public GameObject scroll;
        public Image scrollImage;
        public SpriteLibrary spriteLibrary;
        public GameObject messagePrefab;

        private void Update()
        {
            if (Player.Instance.ActiveAttack != null)
                basicAttackImage.sprite = Player.Instance.ActiveAttack.GetSprite();
            basicAttack.SetActive(Player.Instance.ActiveAttack != null);

            if (Player.Instance.Special != null) specialImage.sprite = Player.Instance.Special.GetSprite();
            special.SetActive(Player.Instance.Special != null);

            if (Player.Instance.Scroll != null) scrollImage.sprite = Player.Instance.Scroll.GetSprite();
            scroll.SetActive(Player.Instance.Scroll != null);
        }

        public void DisplayMessage(string message, MessageTone tone = MessageTone.Positive)
        {
            var messageObject = Instantiate(messagePrefab, transform);
            var textComponent = messageObject.GetComponent<TMP_Text>();
            textComponent.text = message;
            if (tone == MessageTone.Positive) textComponent.color = Color.green;
            else if (tone == MessageTone.Negative) textComponent.color = Color.red;
            // TODO: destroy message object after it is displayed
        }
    }
}