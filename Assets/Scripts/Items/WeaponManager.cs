using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private MeleeWeaponDamageCollider meleeWeaponDamageCollider;

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
        }
    }
}

