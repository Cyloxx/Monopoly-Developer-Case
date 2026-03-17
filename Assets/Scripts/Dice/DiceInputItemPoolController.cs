using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceInputItemPoolController : MonoBehaviour
    {
        [SerializeField] private DiceInputItemUI diceInputItemPrefab;
        [SerializeField] private Transform itemParent;

        private readonly List<DiceInputItemUI> activeItems = new List<DiceInputItemUI>();
        private readonly List<DiceInputItemUI> inactiveItems = new List<DiceInputItemUI>();

        public IReadOnlyList<DiceInputItemUI> ActiveItems => activeItems;

        public void SetActiveItemCount(int targetCount)
        {
            targetCount = Mathf.Max(0, targetCount);
            CleanupNullReferences();

            while (activeItems.Count < targetCount)
            {
                DiceInputItemUI item = GetOrCreateItem();

                if (item == null)
                {
                    Debug.LogError("[DiceInputItemPoolController] GetOrCreateItem returned null.");
                    return;
                }

                item.gameObject.SetActive(true);
                activeItems.Add(item);
            }

            while (activeItems.Count > targetCount)
            {
                int lastIndex = activeItems.Count - 1;
                DiceInputItemUI item = activeItems[lastIndex];
                activeItems.RemoveAt(lastIndex);

                if (item != null)
                {
                    item.gameObject.SetActive(false);
                    inactiveItems.Add(item);
                }
            }

            for (int i = 0; i < activeItems.Count; i++)
            {
                if (activeItems[i] != null)
                {
                    activeItems[i].Configure(i + 1);
                }
            }
        }

        private DiceInputItemUI GetOrCreateItem()
        {
            if (inactiveItems.Count > 0)
            {
                int lastIndex = inactiveItems.Count - 1;
                DiceInputItemUI reusedItem = inactiveItems[lastIndex];
                inactiveItems.RemoveAt(lastIndex);

                if (reusedItem != null)
                {
                    Transform parent = itemParent != null ? itemParent : transform;
                    reusedItem.transform.SetParent(parent, false);
                    return reusedItem;
                }
            }

            if (diceInputItemPrefab == null)
            {
                Debug.LogError("[DiceInputItemPoolController] Dice input item prefab reference is missing.");
                return null;
            }

            Transform targetParent = itemParent != null ? itemParent : transform;
            DiceInputItemUI createdItem = Instantiate(diceInputItemPrefab, targetParent);

            if (createdItem == null)
            {
                Debug.LogError("[DiceInputItemPoolController] Failed to instantiate dice input item.");
                return null;
            }

            createdItem.gameObject.SetActive(false);
            return createdItem;
        }

        private void CleanupNullReferences()
        {
            for (int i = activeItems.Count - 1; i >= 0; i--)
            {
                if (activeItems[i] == null)
                {
                    activeItems.RemoveAt(i);
                }
            }

            for (int i = inactiveItems.Count - 1; i >= 0; i--)
            {
                if (inactiveItems[i] == null)
                {
                    inactiveItems.RemoveAt(i);
                }
            }
        }
    }
}