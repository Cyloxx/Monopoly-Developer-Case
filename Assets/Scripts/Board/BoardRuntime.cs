using System.Collections.Generic;

namespace Joker.Monopoly
{
    public class BoardRuntime
    {
        private readonly List<BoardTile> tiles;

        public IReadOnlyList<BoardTile> Tiles => tiles;
        public int TileCount => tiles.Count;

        public BoardRuntime(List<BoardTile> tiles)
        {
            this.tiles = tiles;
        }

        public BoardTile GetTile(int index)
        {
            if (tiles == null || tiles.Count == 0)
            {
                return null;
            }

            if (index < 0 || index >= tiles.Count)
            {
                return null;
            }

            return tiles[index];
        }

        public int WrapIndex(int index)
        {
            if (tiles == null || tiles.Count == 0)
            {
                return -1;
            }

            int wrappedIndex = index % tiles.Count;

            if (wrappedIndex < 0)
            {
                wrappedIndex += tiles.Count;
            }

            return wrappedIndex;
        }

        public BoardTile GetWrappedTile(int index)
        {
            int wrappedIndex = WrapIndex(index);

            if (wrappedIndex < 0)
            {
                return null;
            }

            return tiles[wrappedIndex];
        }
    }
}