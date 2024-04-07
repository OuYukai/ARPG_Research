using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace SG
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 10;
        [SerializeField] float rotationSpeed = 15;

        [Header("Dodge")]
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            if (player.IsOwner)
            {
                player.characterNetworkManager.verticalMovement.Value = verticalMovement;
                player.characterNetworkManager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkManager.moveAmount.Value = moveAmount;
            }
            else
            {
                verticalMovement = player.characterNetworkManager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkManager.horizontalMovement.Value;
                moveAmount = player.characterNetworkManager.moveAmount.Value;

                // IF NOT LOCKED ON, PASS MOVE AMOUNT
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);

                // IF LOCKED ON, PASS HORZ AND VERT
            }
        }

        public void HandleAllMovement()
        {
            HandleGroundMovement();
            HandleRotation();
            // AERIAL MOVEMENT
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;

            // CLAMP THE MOVEMENTS
        }

        private void HandleGroundMovement()
        {
            if (!player.canMove)
                return;

            GetMovementValues();

            // OUR MOVE DIRECTION IS BASED ON OUR CAMERAS FACING PERSPECTIVE & OUR MOVEMENT INPUTS
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.playerNetworkManager.isSprinting.Value)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    // MOVE AT A RUNNING SPEED
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    // MOVE AT A WALKING SPEED
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }  
        }

        private void HandleRotation()
        {
            if (!player.canRotate)
                return;

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection = targetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }

            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction)
                return;

            //  IF WE ARE MOVING WHEN ATTEMPT TO DODGE, WE PERFORM A ROLL
            if (PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                //  PERFORM A ROLL ANIMATION
                player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_Forward_01", true, true);
            }
            //  IF WE ARE STATIONARY, WE PERFORM A BACKSTEP
            else
            {
                //  PERFORM A BACKSTEP ANIMATION
                //player.playerAnimatorManager.PlayerTargetActionAnimation("Back_Step_01", true, true);
            }
        }

        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                //  SET SPRINTING TO FALSE
                player.playerNetworkManager.isSprinting.Value = false;
            }

            //  IF WE ARE OUT OF STAMINA, SET SPRINTING TO FALSE

            //  IF WE ARE MOVING, SET SPRINTING TO TRUE
            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;
            }
            //  IF WE ARE STATIONARY/MOVEING SLOWLY, SET SPRINTING TO FALSE
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }

            
        }
    }
}

