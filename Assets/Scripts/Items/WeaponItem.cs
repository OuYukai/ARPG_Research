using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponItem : Item
    {
        //  ANIMATOR CONTROLLER OVERIDE (Change attack animation base on weapon you are currently using)

        [Header("Weapon Model")] 
        public GameObject weaponModel;

        [Header("Weapon Requirements")] 
        public int strengthREQ;
        public int dexREQ;
        public int intREQ;
        public int faithREQ;

        [Header("Weapon Base Damage")] 
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightingDamage = 0;
        
        //  WEAPON GUARD ABSORPTIONS (BLOCKING POWER)

        [Header("Weapon poise")] 
        public float poiseDamage = 10;
        //  OFFENSIVE POISE BONUS WHEN ATTACKING
        
        //  WEAPON MODIFIERS
        //  LIGHT ATTACK MODIFIERS
        //  HEAVY ATTACK MODIFIERS
        //  CRITICAL DAMAGE MODIFIERS ECT

        [Header("Stamina Costs")] 
        public int baseStaminaCost = 20;
        //  RUNNING ATTACK STAMINA COST MODIFIER
        //  LIGHT ATTACK STAMINA COST MODIFIER
        //  HEAVY ATTACK STAMINA COST MODIFIER ECT
        
        //  ITEM BASE ACTIONS (RB, RT, LB, LT)
        
        //  ASH OF WAR 
        
        //  BLOCKING SOUNDS
        
    }
}

