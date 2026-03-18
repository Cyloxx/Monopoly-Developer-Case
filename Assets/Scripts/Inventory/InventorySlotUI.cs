using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class InventorySlotUI : MonoBehaviour
    {
        public Image iconImage;

        [SerializeField]
        private TextMeshProUGUI countText;

        private ItemDataSO currentItem;

        public void Initialize(ItemDataSO item, int quantity)
        {
            currentItem = item;

            if (item != null)
            {
                iconImage.sprite = item.icon;
                iconImage.enabled = true;
                countText.text = quantity.ToString();
            }
            else
            {
                iconImage.sprite = null;
                iconImage.enabled = false;
                countText.text = "0";
            }
        }
        public void UpdateQuantity(int newQuantity)
        {
            countText.text = newQuantity.ToString();
        }
    }
}