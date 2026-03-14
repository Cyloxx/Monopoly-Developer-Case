using UnityEngine;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Joker/Monopoly/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public int defaultQuantity = 0;
    }
}