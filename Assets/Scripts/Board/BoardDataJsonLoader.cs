using System;
using UnityEngine;

namespace Joker.Monopoly
{
    public class BoardDataJsonLoader
    {
        public BoardData Load(TextAsset jsonAsset)
        {
            if (jsonAsset == null)
            {
                throw new ArgumentNullException(nameof(jsonAsset), "Board JSON asset is null.");
            }

            if (string.IsNullOrWhiteSpace(jsonAsset.text))
            {
                throw new InvalidOperationException("Board JSON asset is empty.");
            }

            BoardData boardData = JsonUtility.FromJson<BoardData>(jsonAsset.text);

            if (boardData == null)
            {
                throw new InvalidOperationException("Board JSON could not be parsed.");
            }

            if (boardData.tiles == null)
            {
                throw new InvalidOperationException("Board JSON does not contain a valid tiles array.");
            }

            Validate(boardData);

            return boardData;
        }

        private void Validate(BoardData boardData)
        {
            for (int i = 0; i < boardData.tiles.Count; i++)
            {
                TileData tile = boardData.tiles[i];

                if (tile == null)
                {
                    throw new InvalidOperationException($"Tile at index {i} is null.");
                }

                if (tile.tileIndex < 0)
                {
                    throw new InvalidOperationException($"Tile index can not be negative. Problematic tile list index: {i}");
                }

                if (tile.rewardAmount < 0)
                {
                    throw new InvalidOperationException($"Reward amount can not be negative. Tile index: {tile.tileIndex}");
                }

                if (tile.rewardType == TileRewardType.None && tile.rewardAmount > 0)
                {
                    Debug.LogWarning($"Tile {tile.tileIndex} has reward type None but reward amount is greater than zero.");
                }

                if (tile.rewardType != TileRewardType.None &&
                    tile.rewardType != TileRewardType.Question &&
                    tile.rewardAmount == 0)
                {
                    Debug.LogWarning($"Tile {tile.tileIndex} has a reward type but reward amount is zero.");
                }
            }
        }
    }
}