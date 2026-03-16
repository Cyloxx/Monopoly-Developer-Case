using UnityEngine;

namespace Joker.Monopoly
{
    public class BoardRuntimeDebugTester : MonoBehaviour
    {
        [SerializeField] private BoardGenerator boardGenerator;
        [SerializeField] private int testTileIndex = 0;
        [SerializeField] private int wrappedTestIndex = 0;

        [ContextMenu("Print Runtime Summary")]
        private void PrintRuntimeSummary()
        {
            if (boardGenerator == null)
            {
                Debug.LogError("[BoardRuntimeDebugTester] BoardGenerator reference is missing.");
                return;
            }

            if (boardGenerator.Runtime == null)
            {
                Debug.LogError("[BoardRuntimeDebugTester] BoardRuntime is null. Generate the board first.");
                return;
            }

            Debug.Log($"[BoardRuntimeDebugTester] Tile count: {boardGenerator.Runtime.TileCount}");

            for (int i = 0; i < boardGenerator.Runtime.TileCount; i++)
            {
                BoardTile tile = boardGenerator.Runtime.GetTile(i);

                if (tile == null)
                {
                    Debug.LogWarning($"[BoardRuntimeDebugTester] Tile at runtime index {i} is null.");
                    continue;
                }

                TileData tileData = tile.TileData;
                Vector3 worldPosition = tile.GetWorldPosition();

                Debug.Log(
                    $"[BoardRuntimeDebugTester] RuntimeIndex: {i}, TileIndex: {tileData.tileIndex}, RewardType: {tileData.rewardType}, RewardAmount: {tileData.rewardAmount}, Position: {worldPosition}");
            }
        }

        [ContextMenu("Print Tile By Index")]
        private void PrintTileByIndex()
        {
            if (boardGenerator == null)
            {
                Debug.LogError("[BoardRuntimeDebugTester] BoardGenerator reference is missing.");
                return;
            }

            if (boardGenerator.Runtime == null)
            {
                Debug.LogError("[BoardRuntimeDebugTester] BoardRuntime is null. Generate the board first.");
                return;
            }

            BoardTile tile = boardGenerator.Runtime.GetTile(testTileIndex);

            if (tile == null)
            {
                Debug.LogWarning($"[BoardRuntimeDebugTester] No tile found at index {testTileIndex}.");
                return;
            }

            Debug.Log(
                $"[BoardRuntimeDebugTester] GetTile({testTileIndex}) => TileIndex: {tile.TileIndex}, Position: {tile.GetWorldPosition()}");
        }

        [ContextMenu("Print Wrapped Tile")]
        private void PrintWrappedTile()
        {
            if (boardGenerator == null)
            {
                Debug.LogError("[BoardRuntimeDebugTester] BoardGenerator reference is missing.");
                return;
            }

            if (boardGenerator.Runtime == null)
            {
                Debug.LogError("[BoardRuntimeDebugTester] BoardRuntime is null. Generate the board first.");
                return;
            }

            int wrappedIndex = boardGenerator.Runtime.WrapIndex(wrappedTestIndex);
            BoardTile tile = boardGenerator.Runtime.GetWrappedTile(wrappedTestIndex);

            if (tile == null)
            {
                Debug.LogWarning($"[BoardRuntimeDebugTester] No wrapped tile found for index {wrappedTestIndex}.");
                return;
            }

            Debug.Log(
                $"[BoardRuntimeDebugTester] WrappedInput: {wrappedTestIndex}, WrappedIndex: {wrappedIndex}, TileIndex: {tile.TileIndex}, Position: {tile.GetWorldPosition()}");
        }
    }
}