﻿using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "A.I/Actions/Attack")]
    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")] 
        [SerializeField] private string attackAnimation;

        [Header("Combo Action")] 
        public AICharacterAttackAction comboAction; //  The combo action of this attack action

        [Header("Action Values")] 
        [SerializeField] private AttackType attackType;
        public int attackWeight = 50;
        //  ATTACK CAN BE REPEATED
        public float actionRecoveryTime = 1.5f; //  The time before the character can make another attack after performing this one
        public float minimumAttackAngle = -35;
        public float maximumAttackAngle = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;

        public void AttempToPerformAction(AICharacterManager aiCharacter)
        {
            aiCharacter.charaterAnimatorManager.PlayerTargetAttackActionAnimation(attackType, attackAnimation, true);
        }
    }
}