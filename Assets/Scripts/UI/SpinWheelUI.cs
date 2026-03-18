using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class SpinWheelUI : MonoBehaviour
    {
        [SerializeField] private Button spinButton;
        [SerializeField] private GameObject wheel;
        [SerializeField] private GameEventsSO gameEvents;
        [SerializeField] private RewardVisualMappingSO rewardVisualMapping;

        public void Spin()
        {
            StartCoroutine(SpinRoutine());
        }

        private IEnumerator SpinRoutine()
        {
            spinButton.interactable = false;

            int rng = Random.Range(0, 3);
            float targetAngle = 720f + (rng * 120f);
            float rotated = 0f;
            float spinSpeed = 720f;

            while (rotated < targetAngle)
            {
                float step = spinSpeed * Time.deltaTime;
                wheel.transform.Rotate(0f, 0f, -step);
                rotated += step;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);

            ItemDataSO rewardItem = null;
            int rewardAmount = 10;

            if (rng == 0)
            {
                rewardVisualMapping.TryGetItem(TileRewardType.Apple, out rewardItem);
            }
            else if (rng == 1)
            {
                rewardVisualMapping.TryGetItem(TileRewardType.Pear, out rewardItem);
            }
            else
            {
                rewardVisualMapping.TryGetItem(TileRewardType.Strawberry, out rewardItem);
            }

            if (rewardItem != null)
            {
                gameEvents.onRewardCollected.Invoke(rewardItem, rewardAmount);

                for (int i = 0; i < rewardAmount; i++)
                {
                    gameEvents.onItemCollected.Invoke(rewardItem);
                }
            }

            spinButton.interactable = true;
            gameObject.SetActive(false);
        }
    }
}