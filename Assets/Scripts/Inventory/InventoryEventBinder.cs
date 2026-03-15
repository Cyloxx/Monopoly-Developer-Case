using UnityEngine;

namespace Joker.Monopoly
{
    public class InventoryEventBinder : MonoBehaviour
    {
        [SerializeField] private GameEventsSO gameEvents;
        [SerializeField] private InventorySO inventory;

        private void OnEnable()
        {
            if (gameEvents == null)
            {
                return;
            }

            gameEvents.onItemCollected.AddListener(HandleItemCollected);
        }

        private void OnDisable()
        {
            if (gameEvents == null)
            {
                return;
            }

            gameEvents.onItemCollected.RemoveListener(HandleItemCollected);
        }

        private void HandleItemCollected(ItemDataSO item)
        {
            inventory.AddItem(item);
        }
    }
}