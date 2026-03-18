using System.Collections;
using UnityEngine;

namespace Joker.Monopoly
{
    public class PlayerToken : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 0.16f;
        [SerializeField] private float hopHeight = 0.35f;
        [SerializeField] private float landingBounceHeight = 0.08f;
        [SerializeField] private float landingBounceDuration = 0.05f;
        [SerializeField] private Animator animator;

        private Coroutine movementCoroutine;

        public void SetPosition(Vector3 worldPosition)
        {
            transform.position = worldPosition;
        }

        public IEnumerator MoveToPositionRoutine(Vector3 targetPosition)
        {
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
            }

            yield return MoveArcCoroutine(targetPosition);
            movementCoroutine = null;
        }

        private IEnumerator MoveArcCoroutine(Vector3 targetPosition)
        {
            SoundController.instance.PlayMoveSound();

            Vector3 startPosition = transform.position;
            float elapsed = 0f;

            animator.SetTrigger("Jump");
            
            while (elapsed < moveDuration)
            {
                float t = elapsed / moveDuration;
                float easedT = EaseInOutCubic(t);

                Vector3 horizontalPosition = Vector3.Lerp(startPosition, targetPosition, easedT);
                float arcOffset = Mathf.Sin(t * Mathf.PI) * hopHeight;

                transform.position = horizontalPosition + Vector3.up * arcOffset;

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;

            Vector3 bounceUpPosition = targetPosition + Vector3.up * landingBounceHeight;
            elapsed = 0f;

            while (elapsed < landingBounceDuration)
            {
                float t = elapsed / landingBounceDuration;
                transform.position = Vector3.Lerp(targetPosition, bounceUpPosition, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;

            while (elapsed < landingBounceDuration)
            {
                float t = elapsed / landingBounceDuration;
                float easedT = EaseOutBounceLike(t);
                transform.position = Vector3.Lerp(bounceUpPosition, targetPosition, easedT);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
        }

        private float EaseInOutCubic(float t)
        {
            return t < 0.5f
                ? 4f * t * t * t
                : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
        }

        private float EaseOutBounceLike(float t)
        {
            return 1f - Mathf.Pow(1f - t, 2f);
        }
    }
}