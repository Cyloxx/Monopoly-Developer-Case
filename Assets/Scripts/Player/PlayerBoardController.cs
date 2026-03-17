using System;
using System.Collections;
using UnityEngine;

namespace Joker.Monopoly
{
    public class PlayerBoardController : MonoBehaviour
    {
        [SerializeField] private BoardGenerator boardGenerator;
        [SerializeField] private PlayerToken playerToken;
        [SerializeField] private Vector3 tokenOffset = new Vector3(0f, 1f, 0f);
        [SerializeField] private int startTileIndex = 0;
        [SerializeField] private float stepDelay;
        [SerializeField] private GameEventsSO gameEvents;
        [SerializeField] private RewardVisualMappingSO rewardVisualMapping;

        private Coroutine movementCoroutine;
        
        public event Action OnMovementStarted;
        public event Action OnMovementCompleted;
        
        public bool IsMoving => isMoving;
        private bool isMoving;
        
        

        public int CurrentTileIndex { get; private set; }

        private void OnEnable()
        {
            if (boardGenerator != null)
            {
                boardGenerator.OnBoardGenerated += HandleBoardGenerated;
            }
        }

        private void OnDisable()
        {
            if (boardGenerator != null)
            {
                boardGenerator.OnBoardGenerated -= HandleBoardGenerated;
            }
        }

        private void Start()
        {
            TryPlaceImmediately();
        }

        [ContextMenu("Place At Start Tile")]
        public void PlaceAtStartTile()
        {
            if (boardGenerator == null)
            {
                Debug.LogError("[PlayerBoardController] BoardGenerator reference is missing.");
                return;
            }

            if (playerToken == null)
            {
                Debug.LogError("[PlayerBoardController] PlayerToken reference is missing.");
                return;
            }

            if (boardGenerator.Runtime == null)
            {
                Debug.LogWarning("[PlayerBoardController] BoardRuntime is null. Waiting for board generation.");
                return;
            }

            SnapToTileIndex(startTileIndex);
        }
        private Vector3 GetTileTargetPosition(int tileIndex)
        {
            if (boardGenerator == null || boardGenerator.Runtime == null)
            {
                Debug.LogWarning("[PlayerBoardController] BoardRuntime is not ready.");
                return Vector3.zero;
            }

            BoardTile targetTile = boardGenerator.Runtime.GetWrappedTile(tileIndex);

            if (targetTile == null)
            {
                Debug.LogError("[PlayerBoardController] Target tile could not be found.");
                return Vector3.zero;
            }

            return targetTile.GetWorldPosition() + tokenOffset;
        }

        private void SnapToTileIndex(int tileIndex)
        {
            if (boardGenerator == null || boardGenerator.Runtime == null)
            {
                Debug.LogWarning("[PlayerBoardController] BoardRuntime is not ready.");
                return;
            }

            if (playerToken == null)
            {
                Debug.LogError("[PlayerBoardController] PlayerToken reference is missing.");
                return;
            }

            BoardTile targetTile = boardGenerator.Runtime.GetWrappedTile(tileIndex);

            if (targetTile == null)
            {
                Debug.LogError("[PlayerBoardController] Target tile could not be found.");
                return;
            }

            CurrentTileIndex = boardGenerator.Runtime.WrapIndex(tileIndex);
            Vector3 targetPosition = targetTile.GetWorldPosition() + tokenOffset;
            playerToken.SetPosition(targetPosition);
        }
        
        [ContextMenu("Move By 2 Step")]
        public void MoveByOneStep()
        {
            MoveBySteps(2);
        }

        public void MoveBySteps(int steps)
        {
            
            
            if (boardGenerator == null || boardGenerator.Runtime == null)
            {
                Debug.LogWarning("[PlayerBoardController] BoardRuntime is not ready.");
                return;
            }

            if (isMoving)
            {
                Debug.LogWarning("[PlayerBoardController] Player is already moving.");
                return;
            }

            if (steps == 0)
            {
                Debug.Log("[PlayerBoardController] Step amount is zero. No movement applied.");
                return;
            }

            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
            }

            movementCoroutine = StartCoroutine(MoveStepByStepCoroutine(steps));
        }
        
        private IEnumerator MoveStepByStepCoroutine(int steps)
        {
            isMoving = true;
            OnMovementStarted?.Invoke();

            int direction = steps > 0 ? 1 : -1;
            int stepCount = Mathf.Abs(steps);

            for (int i = 0; i < stepCount; i++)
            {
                int nextTileIndex = CurrentTileIndex + direction;
                int wrappedTileIndex = boardGenerator.Runtime.WrapIndex(nextTileIndex);
                Vector3 targetPosition = GetTileTargetPosition(wrappedTileIndex);

                CurrentTileIndex = wrappedTileIndex;
                yield return StartCoroutine(playerToken.MoveToPositionRoutine(targetPosition));

                Debug.Log($"[PlayerBoardController] Step {i + 1}/{stepCount}, CurrentTileIndex: {CurrentTileIndex}");

                yield return new WaitForSeconds(stepDelay);
            }

            isMoving = false;
            movementCoroutine = null;

            ResolveLandingReward();

            OnMovementCompleted?.Invoke();

            Debug.Log($"[PlayerBoardController] Movement completed. Final tile index: {CurrentTileIndex}");
        }
        
        private void ResolveLandingReward()
        {
            if (boardGenerator == null || boardGenerator.Runtime == null)
            {
                Debug.LogWarning("[PlayerBoardController] Can not resolve reward because BoardRuntime is not ready.");
                return;
            }

            BoardTile currentTile = boardGenerator.Runtime.GetTile(CurrentTileIndex);

            if (currentTile == null || currentTile.TileData == null)
            {
                Debug.LogWarning("[PlayerBoardController] Current tile data is missing.");
                return;
            }

            TileData tileData = currentTile.TileData;

            if (tileData.rewardType == TileRewardType.None || tileData.rewardAmount <= 0)
            {
                Debug.Log($"[PlayerBoardController] No reward on tile {tileData.tileIndex}.");
                return;
            }

            if (rewardVisualMapping == null)
            {
                Debug.LogError("[PlayerBoardController] RewardVisualMappingSO reference is missing.");
                return;
            }

            if (gameEvents == null)
            {
                Debug.LogError("[PlayerBoardController] GameEventsSO reference is missing.");
                return;
            }

            if (!rewardVisualMapping.TryGetItem(tileData.rewardType, out ItemDataSO rewardItem))
            {
                Debug.LogError($"[PlayerBoardController] No item mapping found for reward type {tileData.rewardType}.");
                return;
            }

            for (int i = 0; i < tileData.rewardAmount; i++)
            {
                gameEvents.onItemCollected.Invoke(rewardItem);
            }

            Debug.Log($"[PlayerBoardController] Collected reward from tile {tileData.tileIndex}: {tileData.rewardAmount} x {rewardItem.itemName}");
        }

        private void HandleBoardGenerated(BoardRuntime runtime)
        {
            PlaceAtStartTile();
        }

        private void TryPlaceImmediately()
        {
            if (boardGenerator != null && boardGenerator.Runtime != null)
            {
                PlaceAtStartTile();
            }
        }
        
        
    }
}