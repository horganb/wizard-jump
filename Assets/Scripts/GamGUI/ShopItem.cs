using Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamGUI
{
    public class ShopItem : MonoBehaviour
    {
        public TMP_Text itemName;
        public TMP_Text itemPrice;
        public Button buyButton;
        public global::Shop.ShopItem ItemData;

        private void Update()
        {
            itemName.text = ItemData.Name();
            itemPrice.text = ItemData.Price().ToString();
            itemPrice.color = CanAfford() ? Color.white : Color.red;
            buyButton.interactable = CanAfford();
        }

        public void OnPurchase()
        {
            if (!CanAfford()) return;
            Player.Instance.sapphire -= ItemData.Price();
            ItemData.OnPurchase();
            SaveManager.Save();
        }

        private bool CanAfford()
        {
            return Player.Instance.sapphire >= ItemData.Price();
        }
    }
}