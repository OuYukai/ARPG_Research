using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] private string heavy_Attack_01 = "Main_Heavy_Attack_01";
        [SerializeField] private string heavy_Attack_02 = "Main_Heavy_Attack_02";
        
        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);
            
            if (!playerPerformingAction.IsOwner)
                return;
            
            //  CHECK FOR STOPS
            if (playerPerformingAction.playerNetworkManager.currentStamina.Value <= 0)
                return;
            
            if (!playerPerformingAction.isGrounded)
                return;
            
            PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            //  IF WE ARE ATTACKING CURRENTLY, AND WE CAN COMBO, PERFORM THE COMBO ATTACK
            if (playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon && playerPerformingAction.isPerformingAction)
            {
                playerPerformingAction.playerCombatManager.canComboWithMainHandWeapon = false;
                
                //  PERFORM AN ATTACK BASED ON THE PREVIOUS ATTACK WE JUST PLAYED
                if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed == heavy_Attack_01)
                {
                    playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.HeavyAttack02, heavy_Attack_02, true);
                }
                else
                {
                    playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
                }
            }
            //  OTHERWISE, IF WE ARE NOT ALREADY ATTACKING JUST PERFORM A REGULAR ATTACK
            else if (!playerPerformingAction.isPerformingAction)
            {
                if (playerPerformingAction.playerNetworkManager.isUsingRightHand.Value)
                {
                    playerPerformingAction.playerAnimatorManager.PlayerTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_Attack_01, true);
                }

                if (playerPerformingAction.playerNetworkManager.isUsingLeftHand.Value)
                {
                
                }
            }
            
            
        }
    }
}

