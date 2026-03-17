using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class DiceZoneLayout : MonoBehaviour
    {
        [Header("Zone Size")]
        [SerializeField] private float zoneWidth = 8f;
        [SerializeField] private float zoneHeight = 6f;

        [Header("Padding")]
        [SerializeField] private float horizontalPadding = 0.35f;
        [SerializeField] private float verticalPadding = 0.35f;

        [Header("Scale")]
        [SerializeField] private float baseScale = 1f;
        [SerializeField] private float scaleAfterTenToTwelve = 0.9f;
        [SerializeField] private float scaleAfterThirteenToSixteen = 0.8f;
        [SerializeField] private float scaleAfterSeventeenToTwenty = 0.72f;
        [SerializeField] private float minScale = 0.7f;

        public void ApplyLayout(IReadOnlyList<DiceView> diceViews, int activeDiceCount)
        {
            if (diceViews == null || diceViews.Count == 0)
            {
                return;
            }

            GetGridSize(activeDiceCount, out int columns, out int rows);
            float scale = GetScaleForDiceCount(activeDiceCount);

            List<Vector3> localPositions = CalculateLocalPositions(activeDiceCount, columns, rows);

            for (int i = 0; i < diceViews.Count; i++)
            {
                if (diceViews[i] == null)
                {
                    continue;
                }

                bool isActive = i < activeDiceCount;
                diceViews[i].gameObject.SetActive(isActive);

                if (!isActive)
                {
                    continue;
                }

                diceViews[i].transform.SetParent(transform, false);
                diceViews[i].SetRestingLocalPosition(localPositions[i]);
                diceViews[i].transform.localScale = Vector3.one * scale;
            }
        }

        private List<Vector3> CalculateLocalPositions(int activeDiceCount, int columns, int rows)
        {
            List<Vector3> positions = new List<Vector3>(activeDiceCount);

            float usableWidth = zoneWidth - horizontalPadding * 2f;
            float usableHeight = zoneHeight - verticalPadding * 2f;

            float cellWidth = usableWidth / columns;
            float cellHeight = usableHeight / rows;

            float top = zoneHeight * 0.5f - verticalPadding;

            int placedCount = 0;

            for (int row = 0; row < rows; row++)
            {
                int remaining = activeDiceCount - placedCount;
                if (remaining <= 0)
                {
                    break;
                }

                int itemsInThisRow = Mathf.Min(columns, remaining);

                float currentRowWidth = itemsInThisRow * cellWidth;
                float rowStartX = -currentRowWidth * 0.5f;

                for (int column = 0; column < itemsInThisRow; column++)
                {
                    float x = rowStartX + cellWidth * (column + 0.5f);
                    float z = top - cellHeight * (row + 0.5f);

                    positions.Add(new Vector3(x, 0f, z));
                    placedCount++;
                }
            }

            return positions;
        }

        private void GetGridSize(int diceCount, out int columns, out int rows)
        {
            if (diceCount <= 1)
            {
                columns = 1;
                rows = 1;
            }
            else if (diceCount == 2)
            {
                columns = 2;
                rows = 1;
            }
            else if (diceCount <= 4)
            {
                columns = 2;
                rows = 2;
            }
            else if (diceCount <= 6)
            {
                columns = 3;
                rows = 2;
            }
            else if (diceCount <= 9)
            {
                columns = 3;
                rows = 3;
            }
            else if (diceCount <= 12)
            {
                columns = 4;
                rows = 3;
            }
            else if (diceCount <= 16)
            {
                columns = 4;
                rows = 4;
            }
            else
            {
                columns = 4;
                rows = 5;
            }
        }

        private float GetScaleForDiceCount(int diceCount)
        {
            float scale;

            if (diceCount <= 9)
            {
                scale = baseScale;
            }
            else if (diceCount <= 12)
            {
                scale = scaleAfterTenToTwelve;
            }
            else if (diceCount <= 16)
            {
                scale = scaleAfterThirteenToSixteen;
            }
            else
            {
                scale = scaleAfterSeventeenToTwenty;
            }

            return Mathf.Max(minScale, scale);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Matrix4x4 previousMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(zoneWidth, 0.01f, zoneHeight));
            Gizmos.matrix = previousMatrix;
        }
    }
}