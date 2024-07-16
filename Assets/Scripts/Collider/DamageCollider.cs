using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Collider")] 
        [SerializeField] protected Collider damageCollider;
        
        [Header("Damage")] 
        public float physicalDamage = 0; //  (TO DO, SPLIT INTO "Standard", "Strike", "Slash" and "Pierce")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightingDamage = 0;
        public float holyDamage = 0;

        [Header("Contact Point")] 
        protected Vector3 contactPoint;

        [Header("Character Damaged")] 
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        protected virtual void Awake()
        {
            
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();
            
            //  IF YOU WANT TO SEARCH ON BOTH THE DAMAGEABLE CHARACTER COLLIDERS & THE CHARACTER CONTROLLER COLLIDER JUST CHECK FOR NULL HERE AND DO THE FOLLOWING
            //if (damageTarget == null)
            //{
            //    damageTarget = other.GetComponent<CharacterManager>();
            //}

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                
                //  CHECK IF WE DAMAGE THIS TARGET BASED ON FRIENDLY FIRE
                
                //  CHECK IF TARGET IS BLOCKING
                
                //  CHECK IF TARGET IS INVULNERABLE
                
                //  DAMAGE
                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            //  WE DON'T WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK
            //  SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYING DAMAGE
            
            if (charactersDamaged.Contains(damageTarget))
                return;
            
            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectManager.ProcessInstantEffect(damageEffect);
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }
        
        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear(); //   WE RESET THE CHARACTERS THAT HAVE BEEN HIT WHEN WE RESET THE COLLIDER, SO THEY MAY BE HIT AGAIN
        }
    }
}

