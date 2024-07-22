using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Loading;
using UnityEngine;
using Unity.Netcode;
using Random = UnityEngine.Random;

namespace SG
{
    public class CharaterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;

        int vertical;
        int horizontal;

        [Header("Damage Animations")] 
        public string lastDamageAnimationPlayed;
        
        [SerializeField] string hit_Forward_Medium_01 = "Hit_Forward_Medium_01";
        [SerializeField] string hit_Forward_Medium_02 = "Hit_Forward_Medium_02";
        
        [SerializeField] string hit_Backward_Medium_01 = "Hit_Backward_Medium_01";
        [SerializeField] string hit_Backward_Medium_02 = "Hit_Backward_Medium_02";
        
        [SerializeField] string hit_Left_Medium_01 = "Hit_Left_Medium_01";
        [SerializeField] string hit_Left_Medium_02 = "Hit_Left_Medium_02";
        
        [SerializeField] string hit_Right_Medium_01 = "Hit_Right_Medium_01";
        [SerializeField] string hit_Right_Medium_02 = "Hit_Right_Medium_02";

        public List<string> forward_Medium_Damage = new List<string>();
        public List<string> backward_Medium_Damage = new List<string>();
        public List<string> left_Medium_Damage = new List<string>();
        public List<string> right_Medium_Damage = new List<string>();

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        protected virtual void Start()
        {
            forward_Medium_Damage.Add(hit_Forward_Medium_01);
            forward_Medium_Damage.Add(hit_Forward_Medium_02);
            
            backward_Medium_Damage.Add(hit_Backward_Medium_01);
            backward_Medium_Damage.Add(hit_Backward_Medium_02);
            
            left_Medium_Damage.Add(hit_Left_Medium_01);
            left_Medium_Damage.Add(hit_Left_Medium_02);
            
            right_Medium_Damage.Add(hit_Right_Medium_01);
            right_Medium_Damage.Add(hit_Right_Medium_02);
        }

        public string GetRandomAnimationFromList(List<string> animationList)
        {
            List<string> finalList = new List<string>();

            foreach (var item in animationList)
            {
                finalList.Add(item);
            }

            //  CHECK IF WE HAVE ALREADY PLAYED THE DAMAGE ANIMATION SO IT DOESN'T REPEAT
            finalList.Remove(lastDamageAnimationPlayed);
            
            //  CHECK THE LIST FOR NULL ENTRIES, AND REMOVE THEM
            for (int i = finalList.Count - 1 ; i > -1; i--)
            {
                if (finalList[i] == null)
                    finalList.RemoveAt(i);
            }

            int randomValue = Random.Range(0, finalList.Count);

            return finalList[randomValue];
        }

        public void UpdateAnimatorMovementParameters(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            #region   OPTION 1

            float snappedHorizontal;
            float snappedVertical;

            //  This if chain will round the horizontal movement to -1, -0.5, 0, 0.5 or 1
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
            else if (horizontalMovement < -0.5f && horizontalMovement >= -1f)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }
            
            //  This if chain will round the vertical movement to -1, -0.5, 0, 0.5 or 1
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
            else if (verticalMovement < -0.5f && verticalMovement >= -1f)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }
            
            if (isSprinting)
            {
                snappedVertical = 2;
            }

            character.animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
            
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
            Debug.Log("PLAYING ANIMATION: "+targetAnimation);
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
        
        public virtual void PlayerTargetAttackActionAnimation(AttackType attackType,
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
            character.characterCombatManager.currentAttackType = attackType;
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

