using System;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySO inventory;
        [SerializeField] private Transform contentParent;
        [SerializeField] private GameObject slotPrefab;

        private readonly Dictionary<ItemDataSO, InventorySlotUI> slotMap = new();

        private void Awake()
        {
        }

        /*private void OnEnable()
        {
            if (inventory == null) return;

            inventory.OnInventoryChanged += RefreshUI;
            RefreshUI();
        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= RefreshUI;
            }
        }

        private void RefreshUI()
        {
            var currentItems = inventory.Items;

            var toRemove = new List<ItemDataSO>();
            foreach (var item in slotMap)
            {
                if (!currentItems.ContainsKey(item.Key))
                {
                    Destroy(item.Value.gameObject);
                    toRemove.Add(item.Key);
                }
            }
            foreach (var item in toRemove) slotMap.Remove(item);

            foreach (var _item in currentItems)
            {
                ItemDataSO item = _item.Key;
                int count = _item.Value;

                if (slotMap.TryGetValue(item, out var slot))
                {
                    slot.UpdateCount(count);
                }
                else
                {
                    GameObject go = Instantiate(slotPrefab, contentParent);
                    InventorySlotUI newSlot = go.GetComponent<InventorySlotUI>();
                    newSlot.Initialize(item, count);
                    slotMap[item] = newSlot;
                }
            }
        }*/
    }
}