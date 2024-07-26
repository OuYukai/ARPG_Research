using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace SG
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Target Information")] 
        public float viewableAngle;
        public Vector3 targetDirection;
        
        [Header("Detection")] 
        public float detectionRadius = 15;
        public float minimumDetectionAngle = -35;
        public float maximumDetectionAngle = 35;
        
        public void FindATargetViaLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null)
                return;

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();
                
                if (targetCharacter == null)
                    continue;
                
                if (targetCharacter == aiCharacter)
                    continue;
                
                if (targetCharacter.isDead.Value)
                    continue;
                
                //  CAN I ATTACK THIS CHARACTER, IF SO, MAKE THEM MY TARGET
                if (WorldUtilityManager.instance.CanIDamageThisTarget(character.characterGroup, targetCharacter.characterGroup))
                {
                    //  IF A POTENTIAL TARGET IS FOUND, IT HAS TO BE IN FRONT OF US
                    Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if (angleOfTarget > minimumDetectionAngle && angleOfTarget < maximumDetectionAngle)
                    {
                        //  LASTLY, WE CHECK FOR ENVIRO BLOCKS
                        if (Physics.Linecast(
                                aiCharacter.characterCombatManager.lockOnTransform.position, 
                                targetCharacter.characterCombatManager.lockOnTransform.position, 
                                WorldUtilityManager.instance.GetEnviroLayerMask()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                        }
                        else
                        {
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                            PivotTowardsTarget(aiCharacter);
                        }
                    }
                }
                
            }
        }

        public void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            //  PLAY A PIVOT ANIMATION DEPENDING ON VIEWABLE ANGLE OF TARGET
            if (aiCharacter.isPerformingAction)
                return;

            if (viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_R_45_01", true);
            }
            else if (viewableAngle <= -20 && viewableAngle >= -60)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_L_45_01", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_R_90_01", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_L_90_01", true);
            }
            else if (viewableAngle >= 110 && viewableAngle <= 145)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_R_135_01", true);
            }
            else if (viewableAngle <= -110 && viewableAngle >= -145)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_L_135_01", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_R_180_01", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.charaterAnimatorManager.PlayerTargetActionAnimation("Turn_L_180_01", true);
            }
                
        }
    }
}