using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceMovementController : MonoBehaviour
    {
        [SerializeField] private DiceRoller diceRoller;
        [SerializeField] private PlayerBoardController playerBoardController;
        [SerializeField] private List<DiceView> diceViews = new List<DiceView>();

        private Coroutine rollAndMoveCoroutine;
        private int currentDiceCount = 2;

        public bool IsProcessingRoll => rollAndMoveCoroutine != null;

        public event Action OnRollProcessingStarted;
        public event Action OnRollProcessingCompleted;

        public void SetDiceCount(int diceCount)
        {
            currentDiceCount = Mathf.Max(1, diceCount);

            for (int i = 0; i < diceViews.Count; i++)
            {
                if (diceViews[i] != null)
                {
                    diceViews[i].gameObject.SetActive(i < currentDiceCount);
                }
            }
        }

        public bool TryApplyDiceInput(List<int> diceValues)
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

            if (IsProcessingRoll)
            {
                Debug.LogWarning("[DiceMovementController] A dice roll is already being processed.");
                return false;
            }

            if (diceValues == null || diceValues.Count != currentDiceCount)
            {
                Debug.LogWarning("[DiceMovementController] Dice input count does not match selected dice count.");
                return false;
            }

            if (!diceRoller.TryCreateRollResult(diceValues, out int totalValue))
            {
                Debug.LogWarning("[DiceMovementController] Dice input is invalid.");
                return false;
            }

            OnRollProcessingStarted?.Invoke();
            rollAndMoveCoroutine = StartCoroutine(PlayDiceAndMoveCoroutine(diceValues, totalValue));
            return true;
        }

        private IEnumerator PlayDiceAndMoveCoroutine(List<int> diceValues, int totalValue)
        {
            List<Coroutine> runningCoroutines = new List<Coroutine>();

            for (int i = 0; i < currentDiceCount; i++)
            {
                if (i >= diceViews.Count || diceViews[i] == null)
                {
                    continue;
                }

                runningCoroutines.Add(StartCoroutine(diceViews[i].RollToValueRoutine(diceValues[i])));
            }

            for (int i = 0; i < runningCoroutines.Count; i++)
            {
                yield return runningCoroutines[i];
            }

            playerBoardController.MoveBySteps(totalValue);
            OnRollProcessingCompleted?.Invoke();
            rollAndMoveCoroutine = null;
        }
    }
}