using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "RewardVisualMapping", menuName = "Joker/Monopoly/Board/Reward Visual Mapping")]
    public class RewardVisualMappingSO : ScriptableObject
    {
        [SerializeField] private List<RewardVisualData> rewardVisuals = new();

        public bool TryGetIcon(TileRewardType rewardType, out Sprite icon)
        {
            foreach (RewardVisualData rewardVisual in rewardVisuals)
            {
                if (rewardVisual.rewardType == rewardType)
                {
                    icon = rewardVisual.icon;
                    return true;
                }
            }

            icon = null;
            return false;
        }
    }
}