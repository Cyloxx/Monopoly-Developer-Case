using System;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{

    [CreateAssetMenu(fileName = "Inventory", menuName = "Joker/Monopoly/Inventory", order = 200)]
    public class InventorySO : ScriptableObject
    {
        
        public ItemDataSO[] loadedItems;
        
        private readonly Dictionary<ItemDataSO, int> inventory = new Dictionary<ItemDataSO, int>();
        public IReadOnlyDictionary<ItemDataSO, int> Items => inventory;
        
        [SerializeField] private GameEventsSO gameEvents;
        public event System.Action OnInventoryChanged;
        
        
        private string itemsResourcesPath = "Items";

        private void OnEnable()
        {
            gameEvents.onItemCollected.AddListener(CollectItem);
        }

        private void OnDisable()
        {
            gameEvents.onItemCollected.RemoveListener(CollectItem);
        }

        private void CollectItem(ItemDataSO item)
        {
            var currentCount = 0;

            if (inventory.ContainsKey(item))
            {
                currentCount = inventory[item];
                inventory[item] = currentCount + 1;
            }
            else
            {
                inventory.Add(item, 1);
            }
            OnInventoryChanged?.Invoke();
        }

        public void LoadAllItems()
        {
            loadedItems = Resources.LoadAll<ItemDataSO>(itemsResourcesPath);

            foreach (var item in loadedItems)
            {
                if (item != null && !inventory.ContainsKey(item))
                {
                    inventory[item] = item.defaultQuantity;
                }
            }
            
            PrintInventoryState();
        }
        
        private void PrintInventoryState()
        {
            foreach (var kvp in inventory)
            {
                var item = kvp.Key;
                int quantity = kvp.Value;
                Debug.Log($"  {item.name} ({item.itemName}): {quantity}");
            }
        }
    }
}