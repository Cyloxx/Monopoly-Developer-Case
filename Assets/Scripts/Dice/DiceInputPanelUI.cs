using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class DiceInputPanelUI : MonoBehaviour
    {
        [SerializeField] private Transform inputContainer;
        [SerializeField] private DiceInputItemUI diceInputItemPrefab;
        [SerializeField] private DiceMovementController diceMovementController;
        [SerializeField] private Button applyButton;
        [SerializeField] private PlayerBoardController playerBoardController;
        [SerializeField] private TextMeshProUGUI feedbackText;

        private readonly List<DiceInputItemUI> inputItems = new List<DiceInputItemUI>();

        private void OnEnable()
        {
            if (playerBoardController != null)
            {
                playerBoardController.OnMovementStarted += HandleMovementStarted;
                playerBoardController.OnMovementCompleted += HandleMovementCompleted;
            }

            if (diceMovementController != null)
            {
                diceMovementController.OnRollProcessingStarted += HandleRollProcessingStarted;
                diceMovementController.OnRollProcessingCompleted += HandleRollProcessingCompleted;
            }
        }

        private void OnDisable()
        {
            if (playerBoardController != null)
            {
                playerBoardController.OnMovementStarted -= HandleMovementStarted;
                playerBoardController.OnMovementCompleted -= HandleMovementCompleted;
            }

            if (diceMovementController != null)
            {
                diceMovementController.OnRollProcessingStarted -= HandleRollProcessingStarted;
                diceMovementController.OnRollProcessingCompleted -= HandleRollProcessingCompleted;
            }
        }

        public void SetDiceCount(int diceCount)
        {
            if (diceCount < 1)
            {
                diceCount = 1;
            }

            ClearInputItems();

            for (int i = 0; i < diceCount; i++)
            {
                DiceInputItemUI item = Instantiate(diceInputItemPrefab, inputContainer);
                item.Configure(i + 1);
                inputItems.Add(item);
            }
        }

        public void ApplyDiceInput()
        {
            if (playerBoardController != null && playerBoardController.IsMoving)
            {
                return;
            }

            if (diceMovementController != null && diceMovementController.IsProcessingRoll)
            {
                return;
            }

            if (diceMovementController == null)
            {
                SetFeedback("Dice movement controller reference is missing.");
                return;
            }

            List<int> diceValues = new List<int>();

            for (int i = 0; i < inputItems.Count; i++)
            {
                DiceInputItemUI inputItem = inputItems[i];

                if (inputItem == null)
                {
                    SetFeedback($"Dice {i + 1} input item is missing.");
                    return;
                }

                if (!inputItem.TryGetValue(out int diceValue))
                {
                    SetFeedback($"Dice {i + 1} must be a valid number.");
                    return;
                }

                diceValues.Add(diceValue);
            }

            bool success = diceMovementController.TryApplyDiceInput(diceValues);

            if (!success)
            {
                SetFeedback("Please enter valid dice values between 1 and 6.");
                return;
            }

            SetFeedback("Dice input applied.");
        }

        private void HandleMovementStarted()
        {
            SetInteractable(false);
        }

        private void HandleMovementCompleted()
        {
            SetInteractable(true);
        }

        private void HandleRollProcessingStarted()
        {
            SetInteractable(false);
        }

        private void HandleRollProcessingCompleted()
        {
            if (playerBoardController == null || !playerBoardController.IsMoving)
            {
                SetInteractable(true);
            }
        }

        private void SetInteractable(bool isInteractable)
        {
            foreach (DiceInputItemUI inputItem in inputItems)
            {
                if (inputItem != null)
                {
                    inputItem.SetInteractable(isInteractable);
                }
            }

            if (applyButton != null)
            {
                applyButton.interactable = isInteractable;
            }
        }

        private void ClearInputItems()
        {
            for (int i = inputItems.Count - 1; i >= 0; i--)
            {
                if (inputItems[i] != null)
                {
                    Destroy(inputItems[i].gameObject);
                }
            }

            inputItems.Clear();
        }

        private void SetFeedback(string message)
        {
            if (feedbackText != null)
            {
                feedbackText.text = message;
            }

            Debug.Log($"[DiceInputPanelUI] {message}");
        }
    }
}