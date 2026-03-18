using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class SpinWheelUI : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Button spinButton;
        [SerializeField] private float rotationSpeed;


        private void Awake()
        {
            root.SetActive(false);
        }
      
        public void Spin()
        {
            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            var rng = Random.Range(0, 3);
            var targetAngle = rng * 120 + 720;
            do
            {
                transform.Rotate(0f, 0f, 1 * Time.deltaTime);
            } 
            while (root.transform.rotation.eulerAngles.z < targetAngle);
            spinButton.interactable = false;
            yield return null;
        }
    }
}