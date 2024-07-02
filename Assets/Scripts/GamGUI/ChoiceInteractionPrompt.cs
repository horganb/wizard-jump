using System.Collections.Generic;
using Interactable;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace GamGUI
{
    public class ChoiceInteractionPrompt : SingletonMonoBehaviour<ChoiceInteractionPrompt>
    {
        public InteractionPrompt interactionPromptPrefab;
        public GameObject separatorPrefab;
        public List<InteractionPrompt> prompts = new();

        public IInteractable ActiveObject;

        private void Start()
        {
            Hide();
        }

        public void Display(Choice[] choices)
        {
            gameObject.SetActive(false);
            var needRerender = choices.Length != prompts.Count;

            if (needRerender)
            {
                prompts = new List<InteractionPrompt>();
                foreach (Transform child in transform)
                    Destroy(child.gameObject);
                for (var i = 0; i < choices.Length; i++)
                {
                    if (i > 0) Instantiate(separatorPrefab, transform);
                    var prompt = Instantiate(interactionPromptPrefab.gameObject, transform)
                        .GetComponent<InteractionPrompt>();
                    prompt.key.text = choices.Length > 1 && i == 0 ? "Q" : "E";
                    prompts.Add(prompt);
                }
            }

            for (var i = 0; i < choices.Length; i++)
                prompts[i].Display(choices[i].ActionText, choices[i].DescriptionText);

            gameObject.SetActive(true);
            if (needRerender) LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        public void Display(string text)
        {
            Display(new[] { new Choice { ActionText = text } });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            prompts = new List<InteractionPrompt>();
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            ActiveObject = null;
        }

        public void Interact(bool alternate)
        {
            ActiveObject?.Interact(alternate);
        }

        public class Choice
        {
            public string ActionText;
            public string DescriptionText;
        }
    }
}