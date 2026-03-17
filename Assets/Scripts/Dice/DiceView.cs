using System.Collections;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceView : MonoBehaviour
    {
        [Header("Drop Animation")] [SerializeField]
        private float spawnHeight;

        [SerializeField] private float dropDuration;
        [SerializeField] private float firstBounceHeight;
        [SerializeField] private float secondBounceHeight;
        [SerializeField] private float firstBounceDuration;
        [SerializeField] private float secondBounceDuration;

        [Header("Spin")] [SerializeField] private float randomSpinDuration;
        [SerializeField] private float settleDuration;
        [SerializeField] private Vector3 randomSpinEulerSpeedMin;
        [SerializeField] private Vector3 randomSpinEulerSpeedMax;

        [Header("Face Rotations")]
        [SerializeField] private Vector3 face1Rotation;
        [SerializeField] private Vector3 face2Rotation;
        [SerializeField] private Vector3 face3Rotation;
        [SerializeField] private Vector3 face4Rotation;
        [SerializeField] private Vector3 face5Rotation;
        [SerializeField] private Vector3 face6Rotation;

        private Vector3 restingLocalPosition;
        private Coroutine rollCoroutine;

        public IEnumerator RollToValueRoutine(int value)
        {
            if (rollCoroutine != null)
            {
                StopCoroutine(rollCoroutine);
            }

            yield return RollToValueCoroutine(value);
            rollCoroutine = null;
        }

        private IEnumerator RollToValueCoroutine(int value)
        {
            if (!TryGetTargetRotation(value, out Quaternion targetRotation))
            {
                Debug.LogError($"[DiceView] Unsupported dice value: {value}");
                yield break;
            }

            Vector3 spinSpeed = new Vector3(
                Random.Range(randomSpinEulerSpeedMin.x, randomSpinEulerSpeedMax.x),
                Random.Range(randomSpinEulerSpeedMin.y, randomSpinEulerSpeedMax.y),
                Random.Range(randomSpinEulerSpeedMin.z, randomSpinEulerSpeedMax.z));

            Vector3 dropStartPosition = restingLocalPosition + Vector3.up * spawnHeight;
            transform.localPosition = dropStartPosition;

            float elapsed = 0f;

            while (elapsed < dropDuration)
            {
                float t = elapsed / dropDuration;
                float easedT = EaseInCubic(t);

                transform.localPosition = Vector3.Lerp(dropStartPosition, restingLocalPosition, easedT);
                transform.Rotate(spinSpeed * Time.deltaTime, Space.Self);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = restingLocalPosition;

            yield return StartCoroutine(PlayBounceCoroutine(firstBounceHeight, firstBounceDuration, spinSpeed * 0.35f));
            yield return StartCoroutine(PlayBounceCoroutine(secondBounceHeight, secondBounceDuration, spinSpeed * 0.18f));

            Quaternion startRotation = transform.rotation;
            elapsed = 0f;

            while (elapsed < settleDuration)
            {
                float t = elapsed / settleDuration;
                float easedT = EaseOutCubic(t);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            transform.localPosition = restingLocalPosition;

            startRotation = transform.rotation;
            elapsed = 0f;

            while (elapsed < settleDuration)
            {
                float t = elapsed / settleDuration;
                float easedT = EaseOutCubic(t);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, easedT);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
            transform.localPosition = restingLocalPosition;
        }
        
        private IEnumerator PlayBounceCoroutine(float bounceHeight, float duration, Vector3 spinSpeed)
        {
            Vector3 bounceTop = restingLocalPosition + Vector3.up * bounceHeight;

            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float easedT = EaseOutCubic(t);

                transform.localPosition = Vector3.Lerp(restingLocalPosition, bounceTop, easedT);
                transform.Rotate(spinSpeed * Time.deltaTime, Space.Self);

                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;

            duration = Random.Range(0.3f, .6f);
            while (elapsed < duration)
            {
                float t = elapsed / duration;
                float easedT = EaseInCubic(t);

                transform.localPosition = Vector3.Lerp(bounceTop, restingLocalPosition, easedT);
                transform.Rotate(spinSpeed * Time.deltaTime, Space.Self);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = restingLocalPosition;
        }
        
        

        private bool TryGetTargetRotation(int value, out Quaternion rotation)
        {
            switch (value)
            {
                case 1:
                    rotation = Quaternion.Euler(face1Rotation.x, this.transform.eulerAngles.y,face1Rotation.z);
                    return true;
                case 2:
                    rotation = Quaternion.Euler(face2Rotation);
                    return true;
                case 3:
                    rotation = Quaternion.Euler(face3Rotation);
                    return true;
                case 4:
                    rotation = Quaternion.Euler(face4Rotation);
                    return true;
                case 5:
                    rotation = Quaternion.Euler(face5Rotation);
                    return true;
                case 6:
                    rotation = Quaternion.Euler(face6Rotation);
                    return true;
                default:
                    rotation = Quaternion.identity;
                    return false;
            }
        }

        private float EaseInCubic(float t)
        {
            return t * t * t;
        }

        private float EaseOutCubic(float t)
        {
            return 1f - Mathf.Pow(1f - t, 3f);
        }

        private float EaseOutBack(float t)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }
        
        public void SetRestingLocalPosition(Vector3 localPosition)
        {
            restingLocalPosition = localPosition;
            transform.localPosition = localPosition;
        }
    }
}