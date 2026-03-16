using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceRoller : MonoBehaviour
    {
        [SerializeField] private int minValue = 1;
        [SerializeField] private int maxValue = 6;

        public bool TryCreateRollResult(int firstDiceValue, int secondDiceValue, out int totalValue)
        {
            totalValue = 0;

            if (!IsWithinRange(firstDiceValue))
            {
                Debug.LogWarning($"[DiceRoller] First dice value is out of range: {firstDiceValue}");
                return false;
            }

            if (!IsWithinRange(secondDiceValue))
            {
                Debug.LogWarning($"[DiceRoller] Second dice value is out of range: {secondDiceValue}");
                return false;
            }

            totalValue = firstDiceValue + secondDiceValue;

            Debug.Log($"[DiceRoller] Dice result created. Dice1: {firstDiceValue}, Dice2: {secondDiceValue}, Total: {totalValue}");
            return true;
        }

        private bool IsWithinRange(int value)
        {
            return value >= minValue && value <= maxValue;
        }
    }
}