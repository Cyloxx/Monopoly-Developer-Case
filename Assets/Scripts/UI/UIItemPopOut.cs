using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class UIItemPopOut : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] TMP_Text quantityText;
        [SerializeField] private CanvasGroup canvasGroup;   
        [SerializeField] private float fadeDuration;

        private void Start()
        {
            StartCoroutine(Sequence());
        }
    
        public void Initialize(ItemDataSO itemData, int amount)
        {
            if (iconImage != null)
            {
                iconImage.sprite = itemData.icon;
                iconImage.enabled = itemData.icon != null;
            }

            if (quantityText != null)
            {
                quantityText.text = $"+{amount}";
            }
        }

        IEnumerator Sequence()
        {
            StartCoroutine(FadeRoutine(0f, 1f));
            yield return new WaitForSeconds(2*fadeDuration);
            StartCoroutine(FadeRoutine(1f, 0f));
            yield return new WaitForSeconds(fadeDuration);
            Destroy(this);
        }
        private IEnumerator FadeRoutine(float startAlpha, float endAlpha)
        {
            float elapsedTime = 0f;
            canvasGroup.alpha = startAlpha;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
                yield return null;
            }
        }
    }
}

