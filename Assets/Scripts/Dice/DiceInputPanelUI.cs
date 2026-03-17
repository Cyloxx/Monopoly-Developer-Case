using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class DiceInputPanelUI : MonoBehaviour
    {
        [SerializeField] private DiceInputItemPoolController inputItemPoolController;
        [SerializeField] private DiceMovementController diceMovementController;
        [SerializeField] private Button applyButton;
        [SerializeField] private PlayerBoardController playerBoardController;
        [SerializeField] private TextMeshProUGUI feedbackText;

        [SerializeField] private TMP_Dropdown diceCountDropdown;

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
            if (inputItemPoolController == null)
            {
                Debug.LogError("[DiceInputPanelUI] Input item pool controller reference is missing.");
                return;
            }

            inputItemPoolController.SetActiveItemCount(diceCount);
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

            var activeItems = inputItemPoolController.ActiveItems;
            List<int> diceValues = new List<int>();

            for (int i = 0; i < activeItems.Count; i++)
            {
                DiceInputItemUI inputItem = activeItems[i];

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
            if (inputItemPoolController != null)
            {
                var activeItems = inputItemPoolController.ActiveItems;

                for (int i = 0; i < activeItems.Count; i++)
                {
                    if (activeItems[i] != null)
                    {
                        activeItems[i].SetInteractable(isInteractable);
                    }
                }
            }

            if (applyButton != null)
            {
                applyButton.interactable = isInteractable;
            }

            if (diceCountDropdown != null)
            {
                diceCountDropdown.interactable = isInteractable;
            }
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