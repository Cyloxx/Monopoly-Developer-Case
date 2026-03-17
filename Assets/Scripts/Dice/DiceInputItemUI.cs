using TMPro;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceInputItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diceLabelText;
        [SerializeField] private TMP_InputField diceInputField;

        public TMP_InputField InputField => diceInputField;

        public void Configure(int diceIndex)
        {
            if (diceLabelText != null)
            {
                diceLabelText.text = $"Dice {diceIndex}";
            }

            if (diceInputField != null)
            {
                diceInputField.text = string.Empty;
            }
        }

        public bool TryGetValue(out int value)
        {
            value = 0;

            if (diceInputField == null)
            {
                return false;
            }

            string rawText = diceInputField.text;

            if (string.IsNullOrWhiteSpace(rawText))
            {
                return false;
            }

            return int.TryParse(rawText, out value);
        }

        public void SetInteractable(bool isInteractable)
        {
            if (diceInputField != null)
            {
                diceInputField.interactable = isInteractable;
            }
        }
    }
}