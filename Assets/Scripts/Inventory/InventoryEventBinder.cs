using UnityEngine;

namespace Joker.Monopoly
{
    public class InventoryEventBinder : MonoBehaviour
    {
        [SerializeField] private GameEventsSO gameEvents;
        [SerializeField] private InventorySO inventory;
        [SerializeField] private UIItemPopOut popOutPrefab;
        [SerializeField] private Transform popOutParent;
        private void OnEnable()
        {
            if (gameEvents == null)
            {
                return;
            }

            gameEvents.onItemCollected.AddListener(HandleItemCollected);
            gameEvents.onRewardCollected.AddListener(HandleRewardCollected);
            
        }

        private void OnDisable()
        {
            if (gameEvents == null)
            {
                return;
            }

            gameEvents.onItemCollected.RemoveListener(HandleItemCollected);
            gameEvents.onRewardCollected.RemoveListener(HandleRewardCollected);
            
        }

        private void HandleItemCollected(ItemDataSO item)
        {
            inventory.AddItem(item);
        }
        
        private void HandleRewardCollected(ItemDataSO itemData, int amount)
        {
            if (itemData == null || popOutPrefab == null)
            {
                return;
            }

            Transform parent = popOutParent != null ? popOutParent : transform;
            UIItemPopOut popOutInstance = Instantiate(popOutPrefab, parent);
            popOutInstance.Initialize(itemData, amount);
            SoundController.instance.PlayItemObtainedSound();
            
        }
    }
}