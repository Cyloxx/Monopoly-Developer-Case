using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySO inventory;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject slotPrefab;

        private readonly Dictionary<ItemDataSO, InventorySlotUI> _itemSlot = new();

        private void Start()
        {
            CreateSlots();
            inventory.OnInventoryChanged += RefreshUI;
            RefreshUI(); 
        }

        private void OnDestroy()
        {
            inventory.OnInventoryChanged -= RefreshUI;
        }

        private void CreateSlots()
        {
            
            foreach (ItemDataSO item in inventory.Items.Keys)
            {
                GameObject go = Instantiate(slotPrefab, contentParent);
                InventorySlotUI slot = go.GetComponent<InventorySlotUI>();

                slot.Initialize(item, item.defaultQuantity);
                _itemSlot[item] = slot;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
        }

        private void RefreshUI()
        {
            foreach (var pair in _itemSlot)
            {
                ItemDataSO item = pair.Key;
                InventorySlotUI slot = pair.Value;

                if (slot == null) continue;

                int quantity = inventory.Items[item];
                slot.UpdateQuantity(quantity);
            }
        }
    }
}