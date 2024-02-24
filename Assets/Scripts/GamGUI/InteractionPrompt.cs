using TMPro;
using UnityEngine;

namespace GamGUI
{
    public class InteractionPrompt : MonoBehaviour
    {
        public TMP_Text action;
        public TMP_Text description;

        public GameObject activeObject;

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
            activeObject = null;
        }

        public void Interact()
        {
            if (activeObject) activeObject.GetComponent<Chest>().Interact();
        }
    }
}