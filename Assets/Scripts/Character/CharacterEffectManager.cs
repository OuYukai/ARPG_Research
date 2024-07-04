using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CharacterEffectManager : MonoBehaviour
    {
        //  PROCESS INSTANT EFFECTS (TAKE DAMAGE, HEAL)
        
        //  PROCESS TIMED EFFECTS (POISON, BUILD UPS)
        
        //  PROCESS STATIC EFFECTS (ADDING/REMOVING BUFFS FROM TALISMANS ECT)

        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            //  TAKE IN AN EFFECT
            //  PROCESS IT
            effect.ProcessEffect(character);
        }
    }
}

