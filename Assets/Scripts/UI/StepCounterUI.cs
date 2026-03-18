using System;
using TMPro;
using UnityEngine;

namespace Joker.Monopoly
{
    public class StepCounterUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text countText;

        public void SetText(string txt)
        {
            countText.text = txt;
        }
        public void SetTextDefault()
        {
            countText.text = "READY";
        }
    }
}

