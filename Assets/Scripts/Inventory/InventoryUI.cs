using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySO inventory;
        [SerializeField] private ItemCatalogSO itemCatalog;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject slotPrefab;

        private readonly Dictionary<ItemDataSO, InventorySlotUI> itemSlots = new();

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
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }

            itemSlots.Clear();

            foreach (ItemDataSO item in itemCatalog.Items)
            {
                if (item == null)
                {
                    continue;
                }

                GameObject slotObject = Instantiate(slotPrefab, contentParent);
                InventorySlotUI slot = slotObject.GetComponent<InventorySlotUI>();

                slot.Initialize(item, item.defaultQuantity);
                itemSlots[item] = slot;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent.GetComponent<RectTransform>());
        }

        private void RefreshUI()
        {
            foreach (var pair in itemSlots)
            {
                ItemDataSO item = pair.Key;
                InventorySlotUI slot = pair.Value;

                if (slot == null)
                {
                    continue;
                }

                int quantity = inventory.Items.TryGetValue(item, out int quantityValue) ? quantityValue : 0;
                slot.UpdateQuantity(quantityValue);
            }
        }
    }
}