using UnityEngine;

namespace Joker.Monopoly
{
    public class IdleFloat : MonoBehaviour
    {
        [SerializeField] private float amplitude = 0.05f;
        [SerializeField] private float frequency = 0.5f;

        private Vector3 startLocalPosition;

        private void Awake()
        {
            startLocalPosition = transform.localPosition;
        }

        private void Update()
        {
            float offsetY = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;
            transform.localPosition = startLocalPosition + new Vector3(0f, offsetY, 0f);
        }
    }
}