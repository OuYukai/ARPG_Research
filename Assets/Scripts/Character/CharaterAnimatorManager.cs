using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class CharaterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            #region   OPTION 1

            float horizontalAmount = horizontalMovement;
            float verticalAmount = verticalMovement;

            if (isSprinting)
            {
                verticalAmount = 2;
            }

            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticalAmount, 0.1f, Time.deltaTime);
            
            #endregion
            
            #region   OPTION 2

            /*
            float snappedHorizontal = 0;
            float snappedVertical = 0;

            #region Horizontal
            //  This if chain will round the horizontal movement to -1, 0.5, 0, 0.5 or 1

            if (horizontalMovement > 0 && horizontalMovement <= 0.5f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.5f && horizontalMovement <= 1)
            {
                snappedHorizontal = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement >= -0.5f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.5f && horizontalMovement >= -1)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }

            #endregion

            #region Vertical
            //  This if chain will round the vertical movement to -1, 0.5, 0, 0.5 or 1

            if (verticalMovement > 0 && verticalMovement <= 0.5f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.5f && verticalMovement <= 1)
            {
                snappedVertical = 1;
            }
            else if (verticalMovement < 0 && verticalMovement >= -0.5f)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalMovement < -0.5f && verticalMovement >= -1)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }

            #endregion
            
            character.animator.SetFloat("Horizontal", snappedHorizontal);
            character.animator.SetFloat("Vertical", snappedVertical);
            */

            #endregion
        }

        public virtual void PlayerTargetActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canMove = false, 
            bool canRotate = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            //  CAN BE USED TO STOP CHARACTER FROM ATTEMPTING NEW ACTIONS
            //  FOR EXAMPLE, IF YOU GET DAMAGED, AND BEING PERFORMING A DAMAGE ANIMATION
            //  THIS FLAG WILL TURN TRUE IF YOU ARE STUNNED
            //  WE CAN THEN CHECK FOR THIS BEFORE ATTEMPTING NEW ACTIONS
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            //  TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
            character.characterNetworkManager.NotfyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
        
        public virtual void PlayerTargetAttackActionAnimation(
            string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canMove = false, 
            bool canRotate = false)
        {
            //  KEEP TRACK OF LAST ATTACK PERFORMED (FOR COMBOS)
            //  KEEP TRACK OF CURRENT ATTACK TYPE (LIGHT, HEAVY, ECT)
            //  UPDATE ANIMATION SET TO CURRENT WEAPONS ANIMATIONS
            //  DECIDE IF OUR ATTACK CAN BE PARRIED
            //  TELL THE NETWORK OUR "ISATTACKING" FLAG IS ACTIVE (For counter damage ect)
            
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingAction = isPerformingAction;
            character.canMove = canMove;
            character.canRotate = canRotate;

            //  TELL THE SERVER/HOST WE PLAYED AN ANIMATION, AND TO PLAY THAT ANIMATION FOR EVERYBODY ELSE PRESENT
            character.characterNetworkManager.NotfyTheServerOfAttackActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}

