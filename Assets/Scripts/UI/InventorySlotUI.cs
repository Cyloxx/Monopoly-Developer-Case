using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI countText;

        private ItemDataSO currentItem;

        public void Initialize(ItemDataSO item, int count)
        {
            currentItem = item;
            if (item != null)
            {
                iconImage.sprite = item.icon;
                countText.text = count.ToString();
            }
            else
            {
                iconImage.sprite = null;
                countText.text = "0";
            }
        }

        public void UpdateCount(int newCount)
        {
            countText.text = newCount.ToString();
        }
    }
}