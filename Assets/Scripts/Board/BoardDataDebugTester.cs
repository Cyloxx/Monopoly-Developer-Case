using UnityEngine;

namespace Joker.Monopoly
{
    public class BoardDataDebugTester : MonoBehaviour
    {
        [SerializeField] private BoardDataProviderSO boardDataProvider;

        [ContextMenu("Print Board Data")]
        private void PrintBoardData()
        {
            if (boardDataProvider == null)
            {
                Debug.LogError("[BoardDataDebugTester] BoardDataProviderSO reference is missing.");
                return;
            }

            BoardData boardData = boardDataProvider.GetBoardData();

            if (boardData == null)
            {
                Debug.LogError("[BoardDataDebugTester] Loaded board data is null.");
                return;
            }

            if (boardData.tiles == null)
            {
                Debug.LogError("[BoardDataDebugTester] Tile list is null.");
                return;
            }

            Debug.Log($"[BoardDataDebugTester] Total tile count: {boardData.tiles.Count}");

            for (int i = 0; i < boardData.tiles.Count; i++)
            {
                TileData tile = boardData.tiles[i];

                if (tile == null)
                {
                    Debug.LogWarning($"[BoardDataDebugTester] Tile at list index {i} is null.");
                    continue;
                }

                Debug.Log(
                    $"[BoardDataDebugTester] TileIndex: {tile.tileIndex}, RewardType: {tile.rewardType}, RewardAmount: {tile.rewardAmount}");
            }
        }
    }
}