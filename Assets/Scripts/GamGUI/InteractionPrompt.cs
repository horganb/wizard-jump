using TMPro;
using UnityEngine;

namespace GamGUI
{
    public class InteractionPrompt : MonoBehaviour
    {
        public TMP_Text action;
        public TMP_Text description;
        public TMP_Text key;

        public void Display(string actionText, string descriptionText = null)
        {
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
    }
}