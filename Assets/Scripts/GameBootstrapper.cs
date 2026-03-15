using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class GameBootstrapper : MonoBehaviour
    {
        public static GameBootstrapper Instance { get; private set; }

        [SerializeField] private InventorySO inventory;
        [SerializeField] private ItemCatalogSO itemCatalog;

        private IInventoryPersistence inventoryPersistence;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            inventoryPersistence = new PlayerPrefsInventoryPersistence();
        }

        private void OnEnable()
        {
            inventory.InitializeItems(itemCatalog.Items);

            List<string> itemIds = GetCatalogItemIds();
            Dictionary<string, int> loadedData = inventoryPersistence.Load(itemIds);

            inventory.ApplyLoadedData(loadedData);
            inventory.OnInventoryChanged += SaveInventory;
        }

        private void OnDisable()
        {
            inventory.OnInventoryChanged -= SaveInventory;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void SaveInventory()
        {
            inventoryPersistence.Save(inventory.Items);
        }

        private List<string> GetCatalogItemIds()
        {
            List<string> itemIds = new List<string>();

            foreach (ItemDataSO item in itemCatalog.Items)
            {
                if (item == null || string.IsNullOrWhiteSpace(item.ItemId))
                {
                    continue;
                }

                itemIds.Add(item.ItemId);
            }

            return itemIds;
        }
    }
}