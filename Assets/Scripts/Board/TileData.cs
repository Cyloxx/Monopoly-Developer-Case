using System;

namespace Joker.Monopoly
{
    [Serializable]
    public class TileData
    {
        public int tileIndex;
        public TileRewardType rewardType;
        public int rewardAmount;
    }
}