﻿using UnityEngine;

namespace SG
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        public void RotationTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.aiCharacterNetworkManager.isMoving.Value)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }
    }
}