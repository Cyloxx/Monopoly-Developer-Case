using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Joker.Monopoly
{
    public class BoardTileView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tileIndexText;
        [SerializeField] private TextMeshProUGUI rewardAmountText;
        [SerializeField] private Image rewardIconImage;
        [SerializeField] private Transform modelTransform;

        public void Bind(TileData tileData, RewardVisualMappingSO rewardVisualMapping)
        {
            if (tileData == null)
            {
                Debug.LogError("[BoardTileView] Tile data is null.");
                return;
            }

            tileIndexText.text = tileData.tileIndex.ToString();
            bool hasReward = tileData.rewardType != TileRewardType.None && tileData.rewardAmount > 0;

            rewardAmountText.text = hasReward ? tileData.rewardAmount.ToString() : string.Empty;

            if (hasReward && rewardVisualMapping != null &&
                rewardVisualMapping.TryGetModel(tileData.rewardType, out GameObject model))
            {
                Instantiate(model, modelTransform);
            }

            if (hasReward && rewardVisualMapping != null && rewardVisualMapping.TryGetIcon(tileData.rewardType, out Sprite icon))
            {
                rewardIconImage.sprite = icon;
                rewardIconImage.enabled = icon != null;
            }
            else
            {
                rewardIconImage.sprite = null;
                rewardIconImage.enabled = false;
            }
            
            Debug.Log($"Tile reward type: {tileData.rewardType}, reward amount: {tileData.rewardAmount}");
        }
    }
}