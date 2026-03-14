using UnityEngine;
using UnityEngine.Events;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "GameEvents", menuName = "Joker/Monopoly/GameEvents")]
    public class GameEventsSO : ScriptableObject
    {
        public UnityEvent OnDiceRolled   = new UnityEvent();
        public UnityEvent<ItemDataSO> OnItemCollected = new UnityEvent<ItemDataSO>();
        public UnityEvent<int> OnPlayerMoved  = new UnityEvent<int>();

        public void ClearAllListeners()
        {
            OnDiceRolled.RemoveAllListeners();
            OnItemCollected.RemoveAllListeners();
            OnPlayerMoved.RemoveAllListeners();
        }
    }
}