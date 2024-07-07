using UnityEngine;

namespace GamGUI
{
    public class Shop : MonoBehaviour
    {
        public ShopItem shopItemPrefab;
        public Transform shopItemParent;

        private void OnEnable()
        {
            foreach (var child in GetComponentsInChildren<ShopItem>()) Destroy(child.gameObject);
            var items = Utils.InstantiateAllSubclasses<global::Shop.ShopItem>();
            foreach (var itemData in items)
            {
                var shopItem = Instantiate(shopItemPrefab, shopItemParent);
                shopItem.GetComponent<ShopItem>().ItemData = itemData;
            }

            ChoiceInteractionPrompt.Instance.SetDisabled(true);
        }

        private void OnDisable()
        {
            ChoiceInteractionPrompt.Instance.SetDisabled(false);
        }
    }
}