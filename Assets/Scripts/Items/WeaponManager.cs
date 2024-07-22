using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeWeaponDamageCollider;

        private void Awake()
        {
            meleeWeaponDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon ,WeaponItem weaponItem)
        {
            meleeWeaponDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeWeaponDamageCollider.physicalDamage = weaponItem.physicalDamage;
            meleeWeaponDamageCollider.magicDamage = weaponItem.magicDamage;
            meleeWeaponDamageCollider.fireDamage = weaponItem.fireDamage;
            meleeWeaponDamageCollider.lightingDamage = weaponItem.lightingDamage;
            meleeWeaponDamageCollider.holyDamage = weaponItem.holyDamage;

            meleeWeaponDamageCollider.light_Attack_01_Modifiers = weaponItem.light_Attack_01_Modifier;
            meleeWeaponDamageCollider.heavy_Attack_01_Modifiers = weaponItem.heavy_Attack_01_Modifier;
            meleeWeaponDamageCollider.charge_Attack_01_Modifiers = weaponItem.charge_Attack_01_Modifier;
        }
    }
}

