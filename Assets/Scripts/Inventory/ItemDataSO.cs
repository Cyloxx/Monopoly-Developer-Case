using UnityEngine;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Joker/Monopoly/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        [SerializeField] private string itemId;
        public string ItemId => itemId;
        public string itemName;
        public Sprite icon;
        public int defaultQuantity = 0;
        public GameObject itemPrefab;
    }
}