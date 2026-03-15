using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "ItemCatalog", menuName = "Joker/Monopoly/Inventory/Item Catalog")]
    public class ItemCatalogSO : ScriptableObject
    {
        [SerializeField] private List<ItemDataSO> items = new();

        public IReadOnlyList<ItemDataSO> Items => items;
    }
}