using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class DebugCollectButton : MonoBehaviour
    {
        [SerializeField]
        private InventorySO inventory;

        [SerializeField]
        private ItemDataSO targetItem; 

        [SerializeField]
        private GameEventsSO gameEvents;  // Alternatif yol: event üzerinden tetikle

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClicked);
            }
        }

        private void OnButtonClicked()
        {
            if (gameEvents != null && targetItem != null)
            {
                gameEvents.OnItemCollected.Invoke(targetItem);
                Debug.Log($"[Debug] Collected {targetItem.itemName} via event");
            }
            else
            {
                Debug.LogError("[DebugCollectButton] Missing reference: inventory or gameEvents or targetItem");
            }
        }
    }
}