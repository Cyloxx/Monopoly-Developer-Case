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
        private GameEventsSO gameEvents;  

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            if (_button != null)
            {
                _button.onClick.AddListener(OnButtonClicked);
            }
        }

        private void OnButtonClicked()
        {
            if (gameEvents != null && targetItem != null)
            {
                gameEvents.onItemCollected.Invoke(targetItem);
            }
        }
    }
}