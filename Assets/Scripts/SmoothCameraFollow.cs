using System;
using UnityEngine;

namespace Joker.Monopoly
{
    public class SmoothCameraFollow : MonoBehaviour
    {
        private Transform target;

        private Vector3 offset;
        [Header("Position")]
        [SerializeField] private Vector3 playerOffset;
        [SerializeField] private Vector3 diceOffset;
        
        
        [SerializeField] private float smoothTime;

        [Header("Rotation")] [SerializeField] private bool lookAtTarget;

        [SerializeField] private Transform playerTarget;
        [SerializeField] private Transform diceTarget;
        [SerializeField] private PlayerBoardController playerBoardController;
        

        [SerializeField] private float rotationSmoothSpeed;

        private Vector3 currentVelocity;

        private void Start()
        {
            SetTargetDice();
        }

        private void SetTargetPlayer()
        {
            target = playerTarget;
            offset = playerOffset;
        }

        private void SetTargetDice()
        {
            target = diceTarget;
            offset = diceOffset;
        }

        private void OnEnable()
        {
            playerBoardController.OnMovementStarted += SetTargetPlayer;
            playerBoardController.OnMovementCompleted += SetTargetDice;
        }

        private void OnDisable()
        {
            playerBoardController.OnMovementStarted -= SetTargetPlayer;
            playerBoardController.OnMovementCompleted -= SetTargetDice;
        }

        private void LateUpdate()
        {
            if (!target)
            {
                return;
            }

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);

            if (lookAtTarget)
            {
                Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSmoothSpeed * Time.deltaTime);
            }
        }
    }
}