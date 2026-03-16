using UnityEngine;

namespace Joker.Monopoly
{
    [CreateAssetMenu(fileName = "BoardDataProvider", menuName = "Joker/Monopoly/Board/Board Data Provider")]
    public class BoardDataProviderSO : ScriptableObject
    {
        [SerializeField] private TextAsset boardJson;

        public BoardData GetBoardData()
        {
            BoardDataJsonLoader loader = new BoardDataJsonLoader();
            return loader.Load(boardJson);
        }
    }
}