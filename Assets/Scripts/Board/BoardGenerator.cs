using System;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class BoardGenerator : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private BoardDataProviderSO boardDataProvider;
        [SerializeField] private RewardVisualMappingSO rewardVisualMapping;

        [Header("Prefab")]
        [SerializeField] private GameObject boardTilePrefab;

        [Header("Layout")]
        [SerializeField] private Transform boardRoot;
        [SerializeField] private Vector3 spawnStartPosition = Vector3.zero;
        [SerializeField] private Vector3 spawnDirection = Vector3.right;
        [SerializeField] private float tileSpacing = 2f;

        [Header("Debug")]
        [SerializeField] private bool generateOnStart = true;

        public BoardRuntime Runtime { get; private set; }
        public event Action<BoardRuntime> OnBoardGenerated;

        private void Start()
        {
            if (generateOnStart)
            {
                GenerateBoard();
            }
        }

        [ContextMenu("Generate Board")]
        public void GenerateBoard()
        {
            if (boardDataProvider == null)
            {
                Debug.LogError("[BoardGenerator] BoardDataProviderSO reference is missing.");
                return;
            }

            if (boardTilePrefab == null)
            {
                Debug.LogError("[BoardGenerator] Board tile prefab reference is missing.");
                return;
            }

            if (boardRoot == null)
            {
                Debug.LogError("[BoardGenerator] Board root reference is missing.");
                return;
            }

            ClearBoard();

            BoardData boardData = boardDataProvider.GetBoardData();

            if (boardData == null || boardData.tiles == null || boardData.tiles.Count == 0)
            {
                Debug.LogWarning("[BoardGenerator] Board data is empty.");
                return;
            }

            Vector3 normalizedDirection = spawnDirection.normalized;
            List<BoardTile> spawnedTiles = new List<BoardTile>();

            for (int i = 0; i < boardData.tiles.Count; i++)
            {
                TileData tileData = boardData.tiles[i];
                Vector3 spawnPosition = spawnStartPosition + normalizedDirection * (tileSpacing * i);

                GameObject tileObject = Instantiate(boardTilePrefab, spawnPosition, Quaternion.identity, boardRoot);

                BoardTile boardTile = tileObject.GetComponent<BoardTile>();

                if (boardTile == null)
                {
                    Debug.LogError("[BoardGenerator] Spawned tile prefab does not contain BoardTile component.");
                    continue;
                }

                boardTile.Initialize(tileData, rewardVisualMapping);
                spawnedTiles.Add(boardTile);
            }

            Runtime = new BoardRuntime(spawnedTiles);
            ConfigurePortalTiles();
            ConfigureQuestionTiles();
            OnBoardGenerated?.Invoke(Runtime);
        }

        private void ClearBoard()
        {
            Runtime = null;

            for (int i = boardRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(boardRoot.GetChild(i).gameObject);
            }
        }
        private void ConfigurePortalTiles()
        {
            if (Runtime == null || Runtime.TileCount == 0)
            {
                return;
            }

            for (int i = 0; i < Runtime.TileCount; i++)
            {
                BoardTile tile = Runtime.GetTile(i);

                if (tile != null)
                {
                    tile.SetLeftPortalActive(false);
                    tile.SetRightPortalActive(false);
                }
            }

            BoardTile firstTile = Runtime.GetTile(0);
            BoardTile lastTile = Runtime.GetTile(Runtime.TileCount - 1);

            if (firstTile != null)
            {
                firstTile.SetLeftPortalActive(true);
            }

            if (lastTile != null)
            {
                lastTile.SetRightPortalActive(true);
            }
        }

        private void ConfigureQuestionTiles()
        {
            if (Runtime == null || Runtime.TileCount == 0)
            {
                return;
            }

            for (int i = 0; i < Runtime.TileCount; i++)
            {
                BoardTile tile = Runtime.GetTile(i);

                if (tile == null || tile.TileData == null)
                {
                    continue;
                }

                tile.SetQuestionMarkActive(tile.TileData.rewardType == TileRewardType.Question);
            }
        }
        
    }
}