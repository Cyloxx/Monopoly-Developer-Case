using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class PlayerPrefsInventoryPersistence : IInventoryPersistence
    {
        private const string SaveKeyPrefix = "Inv_";

        public Dictionary<string, int> Load(IEnumerable<string> itemIds)
        {
            Dictionary<string, int> loadedData = new Dictionary<string, int>();

            foreach (string itemId in itemIds)
            {
                if (string.IsNullOrWhiteSpace(itemId))
                {
                    continue;
                }

                string saveKey = GetSaveKey(itemId);

                if (PlayerPrefs.HasKey(saveKey))
                {
                    loadedData[itemId] = PlayerPrefs.GetInt(saveKey);
                }
            }

            return loadedData;
        }

        public void Save(IReadOnlyDictionary<ItemDataSO, int> inventory)
        {
            foreach (var pair in inventory)
            {
                ItemDataSO item = pair.Key;

                if (item == null || string.IsNullOrWhiteSpace(item.ItemId))
                {
                    continue;
                }

                string saveKey = GetSaveKey(item.ItemId);
                PlayerPrefs.SetInt(saveKey, pair.Value);
            }

            PlayerPrefs.Save();
            Debug.Log("[PlayerPrefsInventoryPersistence] Inventory saved.");
        }

        private string GetSaveKey(string itemId)
        {
            return SaveKeyPrefix + itemId;
        }
    }
}