using System.Collections.Generic;

namespace Joker.Monopoly
{
    public interface IInventoryPersistence
    {
        Dictionary<string, int> Load(IEnumerable<string> itemIds);
        void Save(IReadOnlyDictionary<ItemDataSO, int> inventory);
    }
}