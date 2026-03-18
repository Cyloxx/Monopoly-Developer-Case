using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Joker.Monopoly
{
    public class BoardTile : MonoBehaviour
    {
        [SerializeField] private BoardTileView tileView;
        [SerializeField] private Transform rewardVisualRoot;
        [SerializeField] private ParticleSystem collectParticlePrefab;
        [SerializeField] private Transform particleSpawnPoint;
        [SerializeField] private float landingPunchScaleMultiplier = 1.08f;
        [SerializeField] private float landingPunchDuration = 0.08f;
        
        [SerializeField] private GameObject leftPortalVisual;
        [SerializeField] private GameObject rightPortalVisual;
        [SerializeField] private List<GameObject> packs;

        public TileData TileData { get; private set; }
        public int TileIndex => TileData != null ? TileData.tileIndex : -1;

        private Coroutine landingFeedbackCoroutine;
        private Vector3 originalScale;

        private void Awake()
        {
            originalScale = transform.localScale;
            ActivateRandomSkinPack();
        }

        public void ActivateRandomSkinPack()
        {
            foreach (var item in packs)
            {
                item.SetActive(false);
            }
            var rng = Random.Range(0,packs.Count);
            packs[rng].SetActive(true);
        }
       
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
            ShowRewardVisual();
        }

        public Vector3 GetWorldPosition()
        {
            return transform.position;
        }

        public bool HasReward()
        {
            return TileData != null &&
                   TileData.rewardType != TileRewardType.None &&
                   TileData.rewardAmount > 0;
        }

        public void ShowRewardVisual()
        {
            if (rewardVisualRoot == null)
            {
                return;
            }

            rewardVisualRoot.gameObject.SetActive(HasReward());
        }

        public void HideRewardVisual()
        {
            if (rewardVisualRoot == null)
            {
                return;
            }

            rewardVisualRoot.gameObject.SetActive(false);
        }

        public void PlayLandingFeedback()
        {
            if (landingFeedbackCoroutine != null)
            {
                StopCoroutine(landingFeedbackCoroutine);
            }

            landingFeedbackCoroutine = StartCoroutine(PlayLandingPunchRoutine());
        }

        public void PlayRewardCollectedFeedback()
        {
            HideRewardVisual();

            if (collectParticlePrefab != null)
            {
                Transform spawnPoint = particleSpawnPoint != null ? particleSpawnPoint : transform;

                ParticleSystem particleInstance = Instantiate(
                    collectParticlePrefab,
                    spawnPoint.position,
                    collectParticlePrefab.transform.rotation);

                particleInstance.Play();

                float destroyDelay = particleInstance.main.duration;
                destroyDelay += particleInstance.main.startLifetime.constantMax;

                Destroy(particleInstance.gameObject, destroyDelay);
            }
        }

        private IEnumerator PlayLandingPunchRoutine()
        {
            transform.localScale = originalScale;

            Vector3 targetScale = originalScale * landingPunchScaleMultiplier;
            float elapsed = 0f;

            while (elapsed < landingPunchDuration)
            {
                float t = elapsed / landingPunchDuration;
                transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0f;

            while (elapsed < landingPunchDuration)
            {
                float t = elapsed / landingPunchDuration;
                transform.localScale = Vector3.Lerp(targetScale, originalScale, t);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = originalScale;
            landingFeedbackCoroutine = null;
        }
        
        public void SetLeftPortalActive(bool isActive)
        {
            if (leftPortalVisual != null)
            {
                leftPortalVisual.SetActive(isActive);
            }
        }

        public void SetRightPortalActive(bool isActive)
        {
            if (rightPortalVisual != null)
            {
                rightPortalVisual.SetActive(isActive);
            }
        }
    }
    

}