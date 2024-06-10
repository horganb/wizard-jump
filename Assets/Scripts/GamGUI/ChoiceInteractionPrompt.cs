using Interactable;
using Singletons;
using TMPro;

namespace GamGUI
{
    public class ChoiceInteractionPrompt : SingletonMonoBehaviour<ChoiceInteractionPrompt>
    {
        public TMP_Text action1;
        public TMP_Text description1;
        public TMP_Text action2;
        public TMP_Text description2;

        public IInteractable ActiveObject;

        private void Start()
        {
            Hide();
        }

        public void Display(string actionText1, string descriptionText1, string actionText2, string descriptionText2)
        {
            gameObject.SetActive(true);

            action1.text = actionText1;
            if (descriptionText1 == null)
            {
                description1.gameObject.SetActive(false);
            }
            else
            {
                description1.gameObject.SetActive(true);
                description1.text = descriptionText1;
            }


            action2.text = actionText2;
            if (descriptionText2 == null)
            {
                description2.gameObject.SetActive(false);
            }
            else
            {
                description2.gameObject.SetActive(true);
                description2.text = descriptionText2;
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ActiveObject = null;
        }

        public void Interact(bool alternate)
        {
            ActiveObject?.Interact(alternate);
        }
    }
}