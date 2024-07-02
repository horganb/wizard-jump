using GamGUI;
using UnityEngine;

namespace Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        protected virtual void Update()
        {
            if (Vector2.Distance(Player.Instance.transform.position, transform.position) <= 1f && CanInteract())
            {
                ChoiceInteractionPrompt.Instance.activeObject = this;
                var choices = PromptChoices();
                if (choices is null)
                    ChoiceInteractionPrompt.Instance.Display(PromptText());
                else
                    ChoiceInteractionPrompt.Instance.Display(choices);
            }
            else
            {
                if (ReferenceEquals(ChoiceInteractionPrompt.Instance.activeObject, this))
                    ChoiceInteractionPrompt.Instance.Hide();
            }
        }

        public abstract void Interact(bool alternate);
        protected abstract string PromptText();

        protected virtual ChoiceInteractionPrompt.Choice[] PromptChoices()
        {
            return null;
        }

        protected virtual bool CanInteract()
        {
            return true;
        }
    }
}