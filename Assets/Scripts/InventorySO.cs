using System;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{

    [CreateAssetMenu(fileName = "Inventory", menuName = "Joker/Monopoly/Inventory", order = 200)]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private ItemDataSO[] loadedItems;
        [SerializeField] private string itemsResourcesPath = "Items";
        private readonly Dictionary<ItemDataSO, int> inventory = new Dictionary<ItemDataSO, int>();
        [SerializeField]
        private GameEventsSO gameEvents;

        private void OnEnable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnItemCollected.AddListener(HandleItemCollected);
                Debug.Log("[InventorySO] OnItemCollected listener added");
            }
            else
            {
                Debug.LogWarning("[InventorySO] GameEventsSO is not assigned!");
            }
        }

        private void OnDisable()
        {
            if (gameEvents != null)
            {
                gameEvents.OnItemCollected.RemoveListener(HandleItemCollected);
                Debug.Log("[InventorySO] OnItemCollected listener removed");
            }
        }

        private void HandleItemCollected(ItemDataSO item)
        {
            if (item == null)
            {
                Debug.LogWarning("[InventorySO] Received null item in HandleItemCollected");
                return;
            }

            if (!inventory.ContainsKey(item))
            {
                // If not initialized yet (rare case), add with 1
                inventory[item] = 1;
            }
            else
            {
                inventory[item]++;
            }

            Debug.Log($"[InventorySO] Item collected: {item.itemName} → New quantity: {inventory[item]}");

            PrintInventoryState();  
        }

        public void LoadAllItems()
        {
            loadedItems = Resources.LoadAll<ItemDataSO>(itemsResourcesPath);

            var count = loadedItems.Length;
            Debug.Log("Items loaded : " + count);
            
            
            for (int i = 0; i < loadedItems.Length; i++)
            {
                var item = loadedItems[i];
                
                Debug.Log($"  [{i+1}] {item?.name} - itemName: '{item?.itemName}'");

                if (item != null && !inventory.ContainsKey(item))
                {
                    inventory[item] = 0;
                }
            }
            
            PrintInventoryState();

        }
        
        private void PrintInventoryState()
        {
            Debug.Log("[InventorySO] Current inventory state:");

            if (inventory.Count == 0)
            {
                Debug.Log("  Inventory is empty.");
                return;
            }

            foreach (var kvp in inventory)
            {
                var item = kvp.Key;
                int quantity = kvp.Value;
                Debug.Log($"  {item.name} ({item.itemName}): {quantity}");
            }
        }
    }
}