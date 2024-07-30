using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")] 
        public CharacterManager characterCausingDamage; //  If the damage is caused by another character attack it will be stored here

        [Header("Damage")] 
        public float physicalDamage = 0; //  (TO DO, SPLIT INTO "Standard", "Strike", "Slash" and "Pierce")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightingDamage = 0;
        public float holyDamage = 0;

        [Header("Final Damage")] 
        private int finalDamageDealt = 0; //   The damage the character takes after all calculations have been made

        [Header("Poise")] 
        public float poiseDamage = 0;
        public bool poiseIsBroken = false; //   If a character's poise is broken, they will be "Stunned" and play a damage animation
        
        //  ( TO DO ) BUILD UPS
        //  build up effect amount

        [Header("Animation")] 
        public bool playerDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Sound FX")] 
        public bool willPlayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; // USED ON TOP OF REGULAR SFX IF THERE IS ELEMENTAL DAMAGE PRESENT (Magic/Fire/Lighting/Holy)

        [Header("Direction Damage Taken From")]
        public float angleHitFrom; //   USED TO DETERMINE WHAT DAMAGE ANIMATION TO PLAY (Move backwards, to the left, to the right ect)
        public Vector3 contactPoint; //  USED TO DETERMINE WHERE THE BLOOD FX INSTANTIATE

        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);
            
            //  IF THE CHARACTER IS DEAD, NO ADDITIONAL DAMAGE EFFECTS SHOULD BE PROCESSED
            if (character.isDead.Value)
                return;
            
            //  CHECK FOR "INVULNERABILITY"
            
            //  CALCULATE DAMAGE
            CalculateDamage(character);
            
            //  CHECK WHICH DIRECTIONAL DAMAGE COME FROM
            //  PLAY A DAMAGE ANIMATION
            PlayDirectionlBasedDamageAnimation(character);
            
            //  CHECK FOR BUILD UPS (POISE BLEED ECT)
            
            //  PLAY DAMAGE SOUND FX
            PlayDamageSFX(character);
            
            //  PLAY DAMAGE VFX (BLOOD)
            PlayDamageVFX(character);
            
            //  IF CHARACTER IS A.I, CHECK FOR NEW TARGET IF CHARACTER CAUSING DAMAGE IS PRESENT
        }

        private void CalculateDamage(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            
            if (character.isDead.Value)
                return;
            
            if (characterCausingDamage != null)
            {
                //  CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE (PHYSICAL/ELEMENTAL DAMAGE BUFF)
            }
            
            //  CHECK CHARACTER FOR FLAT DEFENSES AND SUBTRACT THEM FROM THE DAMAGE
            
            //  CHECK CHARACTER FOR ARMOR ABSORPTIONS, AND SUBTRACT THE PERCENTAGE FROM THE DAMAGE
            
            //  ADD ALL DAMAGE TYPES TOGETHER, AND APPLY FINAL DAMAGE

            finalDamageDealt = Mathf.RoundToInt(physicalDamage + magicDamage + fireDamage + lightingDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }
            
            Debug.Log("FINAL DAMAGE GIVEN " + finalDamageDealt);
            character.characterNetworkManager.currentHealth.Value -= finalDamageDealt;
            
            //  CALCULATE POISE DAMAGE TO DETERMINE IF THE CHARACTER WILL BE STUNNED
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            //  IF WE HAVE FIRE DAMAGE, PLAY FIRE PARTICLES
            //  LIGHTING DAMAGE, LIGHTING PARTICLES ECT
            
            character.characterEffectManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);
            
            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX);
            character.characterSoundFXManager.PlayDamageGrunt();
            //  IF FIRE DAMAGE IS GREATER THAN 0, PLAY BURN SFX
            //  IF LIGHTING DAMAGE IS GREATER THAN 0, PLAY ZAP SFX
        }

        private void PlayDirectionlBasedDamageAnimation(CharacterManager character)
        {
            if (!character.IsOwner)
                return;
            
            // TODO : CALCULATE IF POISE IS BROKEN
            poiseIsBroken = true;
            
            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                //  PLAY FRONT ANIMATION
                damageAnimation = character.charaterAnimatorManager.GetRandomAnimationFromList(character.charaterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                //  PLAY FRONT ANIMATION
                damageAnimation = character.charaterAnimatorManager.GetRandomAnimationFromList(character.charaterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                //  PLAY BACK ANIMATION
                damageAnimation = character.charaterAnimatorManager.GetRandomAnimationFromList(character.charaterAnimatorManager.backward_Medium_Damage);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                //  PLAY LEFT ANIMATION
                damageAnimation = character.charaterAnimatorManager.GetRandomAnimationFromList(character.charaterAnimatorManager.left_Medium_Damage);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                //  PLAY RIGHT ANIMATION
                damageAnimation = character.charaterAnimatorManager.GetRandomAnimationFromList(character.charaterAnimatorManager.right_Medium_Damage);
            }
            
            //  IF POISE IS BROKEN, PLAY A STAGGERING DAMAGE ANIMATION
            if (poiseIsBroken)
            {
                character.charaterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
                character.charaterAnimatorManager.PlayerTargetActionAnimation(damageAnimation, true);
            }
        }
    }
}

