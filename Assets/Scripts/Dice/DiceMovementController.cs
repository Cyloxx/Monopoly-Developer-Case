using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceMovementController : MonoBehaviour
    {
        [SerializeField] private DiceRoller diceRoller;
        [SerializeField] private PlayerBoardController playerBoardController;

        public bool TryApplyDiceInput(int firstDiceValue, int secondDiceValue)
        {
            if (diceRoller == null)
            {
                Debug.LogError("[DiceMovementController] DiceRoller reference is missing.");
                return false;
            }

            if (playerBoardController == null)
            {
                Debug.LogError("[DiceMovementController] PlayerBoardController reference is missing.");
                return false;
            }

            if (!diceRoller.TryCreateRollResult(firstDiceValue, secondDiceValue, out int totalValue))
            {
                Debug.LogWarning("[DiceMovementController] Dice input is invalid.");
                return false;
            }

            playerBoardController.MoveBySteps(totalValue);
            return true;
        }
    }
}