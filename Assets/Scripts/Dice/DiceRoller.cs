using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceRoller : MonoBehaviour
    {
        [SerializeField] private int minValue = 1;
        [SerializeField] private int maxValue = 6;

        public bool TryCreateRollResult(List<int> diceValues, out int totalValue)
        {
            totalValue = 0;

            if (diceValues == null || diceValues.Count == 0)
            {
                Debug.LogWarning("[DiceRoller] Dice value list is null or empty.");
                return false;
            }

            for (int i = 0; i < diceValues.Count; i++)
            {
                int value = diceValues[i];

                if (!IsWithinRange(value))
                {
                    Debug.LogWarning($"[DiceRoller] Dice value at index {i} is out of range: {value}");
                    return false;
                }

                totalValue += value;
            }

            Debug.Log($"[DiceRoller] Roll result created. Dice count: {diceValues.Count}, Total: {totalValue}");
            return true;
        }

        private bool IsWithinRange(int value)
        {
            return value >= minValue && value <= maxValue;
        }
    }
}