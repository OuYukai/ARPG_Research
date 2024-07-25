using UnityEngine;

namespace SG
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Detection")] 
        [SerializeField] float detectionRadius = 15;
        [SerializeField] private float minimumDetectionAngle = -35;
        [SerializeField] private float maximumDetectionAngle = 35;
        
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
                    float viewableAngle = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if (viewableAngle > minimumDetectionAngle && viewableAngle < maximumDetectionAngle)
                    {
                        //  LASTLY, WE CHECK FOR ENVIRO BLOCKS
                        if (Physics.Linecast(
                                aiCharacter.characterCombatManager.lockOnTransform.position, 
                                targetCharacter.characterCombatManager.lockOnTransform.position, 
                                WorldUtilityManager.instance.GetEnviroLayerMask()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                            Debug.Log("BLOCKED");
                        }
                        else
                        {
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        }
                    }
                }
                
            }
        }
    }
}