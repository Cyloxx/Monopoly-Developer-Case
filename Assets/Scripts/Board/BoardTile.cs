using UnityEngine;

namespace Joker.Monopoly
{
    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private BoardTileView tileView;

        public TileData TileData { get; private set; }
        public int TileIndex => TileData != null ? TileData.tileIndex : -1;

        public void Initialize(TileData tileData, RewardVisualMappingSO rewardVisualMapping)
        {
            TileData = tileData;

            if (tileView == null)
            {
                tileView = GetComponent<BoardTileView>();
            }

            if (tileView == null)
            {
                Debug.LogError("[BoardTile] BoardTileView component is missing.");
                return;
            }

            tileView.Bind(tileData, rewardVisualMapping);
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }
    }
}