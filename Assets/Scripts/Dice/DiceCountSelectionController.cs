using TMPro;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceCountSelectionController : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown diceCountDropdown;
        [SerializeField] private DiceInputPanelUI diceInputPanelUI;
        [SerializeField] private DiceMovementController diceMovementController;

        private void Start()
        {
            ApplyCurrentDropdownValue();
        }

        public void HandleDropdownValueChanged(int dropdownIndex)
        {
            ApplyCurrentDropdownValue();
        }

        private void ApplyCurrentDropdownValue()
        {
            if (diceCountDropdown == null)
            {
                Debug.LogError("[DiceCountSelectionController] Dice count dropdown reference is missing.");
                return;
            }

            int selectedDiceCount = diceCountDropdown.value + 1;

            if (diceInputPanelUI != null)
            {
                diceInputPanelUI.SetDiceCount(selectedDiceCount);
            }
    
            if (diceMovementController != null)
            {
                diceMovementController.SetDiceCount(selectedDiceCount);
            }
        }
    }
}