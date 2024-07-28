﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SubsystemsImplementation;

namespace SG
{
    [CreateAssetMenu(menuName = "A.I/States/Pursue Target")]
    public class PursueTargetState : AIState
    {
        public override AIState Tick(AICharacterManager aiCharacter)
        {
            //  CHECK IF WE ARE PERFORMING AN ACTION (IF SO DO NOTHING UNTIL ACTION IS COMPLETE)
            if (aiCharacter.isPerformingAction)
                return this;

            //  CHECK IF OUR TARGET IS NULL, IF WE DO NOT HAVE A TARGET, RETURN TO IDLE STATE
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            //  MAKE SURE OUR NAVMESH AGENT IS ACTIVE, IF IT'S NOT ENABLE IT
            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;
            
            //  IF OUR TARGET GOES OUTSIDE OF THE CHARACTER FOV, PIVOT TO FACE THEM
            if (aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumDetectionAngle
                || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumDetectionAngle)
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            
            aiCharacter.aiCharacterLocomotionManager.RotationTowardsAgent(aiCharacter);

            //  IF WE ARE WITHIN COMBAT RANGE OF A TARGET, SWITCH TO COMBAT STANCE STATE
            //if (aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.combatStance.maximumEngagementDistance)
                //return SwitchState(aiCharacter, aiCharacter.combatStance);
            
                if (aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
                    return SwitchState(aiCharacter, aiCharacter.combatStance);

            //  IF THE TARGET IS NOT REACHABLE, AND THEY ARE FAR AWAY, RETURN HOME

            //  PURSUE THE TARGET
            //  OPTION 01
            //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);

            //  OPTION 02
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }
    }
}