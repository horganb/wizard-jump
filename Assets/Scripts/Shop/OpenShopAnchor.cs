using UnityEngine;

namespace Shop
{
    public class OpenShopAnchor : Interactable.Interactable
    {
        public GameObject shop;

        public override void Interact(bool alternate)
        {
            shop.SetActive(true);
        }

        protected override string PromptText()
        {
            return "Browse Shop";
        }
    }
}