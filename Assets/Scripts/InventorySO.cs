using System;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{

    [CreateAssetMenu(fileName = "Inventory", menuName = "Joker/Monopoly/Inventory", order = 200)]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private GameEventsSO gameEvents;
        private readonly Dictionary<ItemDataSO, int> inventory = new Dictionary<ItemDataSO, int>();
        public IReadOnlyDictionary<ItemDataSO, int> Items => inventory;
        public event System.Action OnInventoryChanged;
        private string _itemsResourcesPath = "Items";
        
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
            if (inventory.TryGetValue(item, out int count))
            {
                inventory[item] = count + 1;
            }
            else
            {
                inventory[item] = 1;
            }

            OnInventoryChanged?.Invoke();
            Save();
        }

        public void LoadAllItems()
        {
            ItemDataSO[] loaded = Resources.LoadAll<ItemDataSO>(_itemsResourcesPath);

            foreach (var item in loaded)
            {
                if (item != null && !inventory.ContainsKey(item))
                {
                    inventory[item] = item.defaultQuantity;
                }
            }

            PrintInventoryState();
            OnInventoryChanged?.Invoke();  
        }

        private void PrintInventoryState()
        {
            Debug.Log("[InventorySO] Current inventory state:");
            if (inventory.Count == 0)
            {
                Debug.Log("  Inventory is empty.");
                return;
            }

            foreach (var _item in inventory)
            {
                var item = _item.Key;
                int quantity = _item.Value;
                Debug.Log($"  {item.name} ({item.itemName}): {quantity}");
            }
        }
        
        public void Save()
        {
            List<ItemDataSO> keys = new List<ItemDataSO>(inventory.Keys);

            foreach (var key in keys)
            {
                if (key == null) continue;

                string saveKey = "Inv_" + key.name;
                PlayerPrefs.SetInt(saveKey, inventory[key]);
            }

            PlayerPrefs.Save();
            Debug.Log("[InventorySO] Saved to PlayerPrefs");
        }
        
        public void Load()
        {
            List<ItemDataSO> keys = new List<ItemDataSO>(inventory.Keys);

            foreach (var key in keys)
            {
                if (!key) continue;

                string saveKey = "Inv_" + key.name;
                int savedValue = PlayerPrefs.GetInt(saveKey, 0);

                inventory[key] = savedValue; 
            }

            OnInventoryChanged?.Invoke();
            Debug.Log("[InventorySO] Loaded from PlayerPrefs");
        }
    }
}