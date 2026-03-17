using System.Collections;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceView : MonoBehaviour
    {
        [Header("Spin")]
        [SerializeField] private float randomSpinDuration = 0.55f;
        [SerializeField] private float settleDuration = 0.3f;
        [SerializeField] private float overshootDuration = 0.08f;
        [SerializeField] private Vector3 randomSpinEulerSpeedMin = new Vector3(650f, 780f, 700f);
        [SerializeField] private Vector3 randomSpinEulerSpeedMax = new Vector3(920f, 1080f, 980f);

        [Header("Face Rotations")]
        [SerializeField] private Vector3 face1Rotation;
        [SerializeField] private Vector3 face2Rotation;
        [SerializeField] private Vector3 face3Rotation;
        [SerializeField] private Vector3 face4Rotation;
        [SerializeField] private Vector3 face5Rotation;
        [SerializeField] private Vector3 face6Rotation;

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

            float elapsed = 0f;

            while (elapsed < randomSpinDuration)
            {
                transform.Rotate(spinSpeed * Time.deltaTime, Space.Self);
                elapsed += Time.deltaTime;
                yield return null;
            }

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

            Quaternion overshootRotation = targetRotation * Quaternion.Euler(4f, -3f, 2f);
            elapsed = 0f;

            while (elapsed < overshootDuration)
            {
                float t = elapsed / overshootDuration;
                transform.rotation = Quaternion.Slerp(targetRotation, overshootRotation, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;

            while (elapsed < overshootDuration)
            {
                float t = elapsed / overshootDuration;
                float easedT = EaseOutBack(t);
                transform.rotation = Quaternion.Slerp(overshootRotation, targetRotation, easedT);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = targetRotation;
        }

        private bool TryGetTargetRotation(int value, out Quaternion rotation)
        {
            switch (value)
            {
                case 1:
                    rotation = Quaternion.Euler(face1Rotation);
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
    }
}