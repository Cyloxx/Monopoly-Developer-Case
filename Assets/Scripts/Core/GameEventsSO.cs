using UnityEngine;
using UnityEngine.Events;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "GameEvents", menuName = "Joker/Monopoly/GameEvents")]
    public class GameEventsSO : ScriptableObject
    {
        public UnityEvent onDiceRolled;
        public UnityEvent<ItemDataSO> onItemCollected;
        public UnityEvent<ItemDataSO, int> onRewardCollected;
        public UnityEvent<int> onPlayerMoved;

        public void ClearAllListeners()
        {
            onDiceRolled.RemoveAllListeners();
            onItemCollected.RemoveAllListeners();
            onPlayerMoved.RemoveAllListeners();
        }
    }
}