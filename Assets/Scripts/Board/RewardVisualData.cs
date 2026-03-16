using System;
using UnityEngine;

namespace Joker.Monopoly
{
    [Serializable]
    public class RewardVisualData
    {
        public TileRewardType rewardType;
        public Sprite icon;
        public ItemDataSO itemData;
    }
}