using System;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "Joker/Monopoly/Inventory", order = 200)]
    public class InventorySO : ScriptableObject
    {
        private readonly Dictionary<ItemDataSO, int> inventory = new Dictionary<ItemDataSO, int>();

        public IReadOnlyDictionary<ItemDataSO, int> Items => inventory;
        public event Action OnInventoryChanged;

        public void InitializeItems(IEnumerable<ItemDataSO> items)
        {
            inventory.Clear();

            foreach (ItemDataSO item in items)
            {
                if (item == null)
                {
                    continue;
                }

                if (!inventory.ContainsKey(item))
                {
                    inventory[item] = item.defaultQuantity;
                }
            }

            OnInventoryChanged?.Invoke();
        }

        public void AddItem(ItemDataSO item, int amount = 1)
        {
            if (item == null)
            {
                return;
            }

            if (amount <= 0)
            {
                return;
            }

            if (inventory.TryGetValue(item, out int currentAmount))
            {
                inventory[item] = currentAmount + amount;
            }
            else
            {
                inventory[item] = amount;
            }

            OnInventoryChanged?.Invoke();
        }

        public void ApplyLoadedData(Dictionary<string, int> loadedData)
        {
            if (loadedData == null)
            {
                return;
            }

            List<ItemDataSO> keys = new List<ItemDataSO>(inventory.Keys);

            foreach (ItemDataSO item in keys)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.ItemId))
                {
                    continue;
                }

                if (loadedData.TryGetValue(item.ItemId, out int loadedAmount))
                {
                    inventory[item] = loadedAmount;
                }
            }

            OnInventoryChanged?.Invoke();
        }

        public void PrintInventoryState()
        {
            Debug.Log("[InventorySO] Current inventory state:");

            if (inventory.Count == 0)
            {
                Debug.Log("  Inventory is empty.");
                return;
            }

            foreach (var pair in inventory)
            {
                ItemDataSO item = pair.Key;
                int quantity = pair.Value;
                Debug.Log($"  {item.name} ({item.itemName}): {quantity}");
            }
        }
    }
}