using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class DiceInputPanelUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField firstDiceInputField;
        [SerializeField] private TMP_InputField secondDiceInputField;
        [SerializeField] private DiceMovementController diceMovementController;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private Button applyButton;
        [SerializeField] private PlayerBoardController playerBoardController;
        
        private void OnEnable()
        {
            if (playerBoardController != null)
            {
                playerBoardController.OnMovementStarted += HandleMovementStarted;
                playerBoardController.OnMovementCompleted += HandleMovementCompleted;
            }
        }

        private void OnDisable()
        {
            if (playerBoardController != null)
            {
                playerBoardController.OnMovementStarted -= HandleMovementStarted;
                playerBoardController.OnMovementCompleted -= HandleMovementCompleted;
            }
        }

        public void ApplyDiceInput()
        {
            if (playerBoardController != null && playerBoardController.IsMoving)
            {
                SetFeedback("Player is already moving.");
                return;
            }
            
            if (diceMovementController == null)
            {
                SetFeedback("Dice movement controller reference is missing.");
                Debug.LogError("[DiceInputPanelUI] DiceMovementController reference is missing.");
                return;
            }

            if (!TryParseInput(firstDiceInputField, out int firstDiceValue, "Dice 1"))
            {
                return;
            }

            if (!TryParseInput(secondDiceInputField, out int secondDiceValue, "Dice 2"))
            {
                return;
            }

            bool success = diceMovementController.TryApplyDiceInput(firstDiceValue, secondDiceValue);

            if (!success)
            {
                SetFeedback("Please enter values between 1 and 6.");
                return;
            }

            SetFeedback($"Applied dice values: {firstDiceValue} + {secondDiceValue}");
        }

        private bool TryParseInput(TMP_InputField inputField, out int value, string fieldName)
        {
            value = 0;

            if (inputField == null)
            {
                SetFeedback($"{fieldName} input field reference is missing.");
                Debug.LogError($"[DiceInputPanelUI] {fieldName} input field reference is missing.");
                return false;
            }

            string rawText = inputField.text;

            if (string.IsNullOrWhiteSpace(rawText))
            {
                SetFeedback($"{fieldName} is empty.");
                return false;
            }

            if (!int.TryParse(rawText, out value))
            {
                SetFeedback($"{fieldName} must be a number.");
                return false;
            }

            return true;
        }

        private void SetFeedback(string message)
        {
            if (feedbackText != null)
            {
                feedbackText.text = message;
            }

            Debug.Log($"[DiceInputPanelUI] {message}");
        }
        
        private void HandleMovementStarted()
        {
            SetInteractable(false);
            SetFeedback("Player is moving...");
        }

        private void HandleMovementCompleted()
        {
            SetInteractable(true);
            SetFeedback("Movement completed.");
        }

        private void SetInteractable(bool isInteractable)
        {
            if (firstDiceInputField != null)
            {
                firstDiceInputField.interactable = isInteractable;
            }

            if (secondDiceInputField != null)
            {
                secondDiceInputField.interactable = isInteractable;
            }

            if (applyButton != null)
            {
                applyButton.interactable = isInteractable;
            }
        }
    }
}