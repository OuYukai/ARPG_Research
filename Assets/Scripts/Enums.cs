using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{

}

public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    No_SLOT,
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHand,
    //Right Hips,
    //Left Hips,
    //Back
}

//  THIS IS USED TO CALCULATE DAMAGE BASED ON ATTACK TYPE
public enum AttackType
{
    LightAttack01,
    LightAttack02,
    HeavyAttack01,
    HeavyAttack02,
    ChargeAttack01,
    ChargeAttack02,
}