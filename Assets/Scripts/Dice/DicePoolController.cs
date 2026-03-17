using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DicePoolController : MonoBehaviour
    {
        [SerializeField] private GameObject dicePrefab;
        [SerializeField] private Transform diceParent;

        private readonly List<DiceView> activeDiceViews = new List<DiceView>();
        private readonly List<DiceView> inactiveDiceViews = new List<DiceView>();

        public IReadOnlyList<DiceView> ActiveDiceViews => activeDiceViews;

        public void SetActiveDiceCount(int targetCount)
        {
            targetCount = Mathf.Max(0, targetCount);

            while (activeDiceViews.Count < targetCount)
            {
                DiceView diceView = GetOrCreateDice();

                if (diceView == null)
                {
                    Debug.LogError("[DicePoolController] GetOrCreateDice returned null.");
                    return;
                }

                GameObject rootObject = diceView.gameObject;
                rootObject.SetActive(true);
                activeDiceViews.Add(diceView);
            }

            while (activeDiceViews.Count > targetCount)
            {
                int lastIndex = activeDiceViews.Count - 1;
                DiceView diceView = activeDiceViews[lastIndex];
                activeDiceViews.RemoveAt(lastIndex);

                if (diceView != null)
                {
                    diceView.gameObject.SetActive(false);
                    inactiveDiceViews.Add(diceView);
                }
            }
        }

        private DiceView GetOrCreateDice()
        {
            CleanupNullReferences();

            if (inactiveDiceViews.Count > 0)
            {
                int lastIndex = inactiveDiceViews.Count - 1;
                DiceView reusedDice = inactiveDiceViews[lastIndex];
                inactiveDiceViews.RemoveAt(lastIndex);

                if (reusedDice != null)
                {
                    Transform parent = diceParent != null ? diceParent : transform;
                    reusedDice.transform.SetParent(parent, false);
                    return reusedDice;
                }
            }

            if (dicePrefab == null)
            {
                Debug.LogError("[DicePoolController] Dice prefab reference is missing.");
                return null;
            }

            Transform targetParent = diceParent != null ? diceParent : transform;
            GameObject createdObject = Instantiate(dicePrefab, targetParent);

            if (createdObject == null)
            {
                Debug.LogError("[DicePoolController] Instantiate returned null.");
                return null;
            }

            DiceView createdDice = createdObject.GetComponent<DiceView>();

            if (createdDice == null)
            {
                Debug.LogError("[DicePoolController] Instantiated prefab does not contain a DiceView component on its root.");
                Destroy(createdObject);
                return null;
            }

            createdObject.SetActive(false);
            return createdDice;
        }

        private void CleanupNullReferences()
        {
            for (int i = activeDiceViews.Count - 1; i >= 0; i--)
            {
                if (activeDiceViews[i] == null)
                {
                    activeDiceViews.RemoveAt(i);
                }
            }

            for (int i = inactiveDiceViews.Count - 1; i >= 0; i--)
            {
                if (inactiveDiceViews[i] == null)
                {
                    inactiveDiceViews.RemoveAt(i);
                }
            }
        }
    }
}