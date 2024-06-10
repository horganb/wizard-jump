using Interactable;
using Singletons;
using TMPro;

namespace GamGUI
{
    public class InteractionPrompt : SingletonMonoBehaviour<InteractionPrompt>
    {
        public TMP_Text action;
        public TMP_Text description;

        public IInteractable ActiveObject;

        private void Start()
        {
            Hide();
        }

        public void Display(string actionText, string descriptionText = null)
        {
            gameObject.SetActive(true);
            action.text = actionText;
            if (descriptionText == null)
            {
                description.gameObject.SetActive(false);
            }
            else
            {
                description.gameObject.SetActive(true);
                description.text = descriptionText;
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