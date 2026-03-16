using System;
using System.Collections.Generic;

namespace Joker.Monopoly
{
    [Serializable]
    public class BoardData
    {
        public List<TileData> tiles = new List<TileData>();
    }
}