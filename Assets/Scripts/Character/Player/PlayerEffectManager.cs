using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerEffectManager : CharacterEffectManager
    {
        [Header("Debug Delete Later")] 
        [SerializeField] private InstantCharacterEffect effectToTest;
        [SerializeField] private bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                //  WHEN WE INSTANTIATE IT, THE ORIGINAL IS NOT EFFECTED
                InstantCharacterEffect effect = Instantiate(effectToTest);
                
                //  WHEN WE DONT INSTANTIATE IT, THE ORIGINAL IS CHANGED (YOU DO NOT WANT THIS IN MOST CASES)
                //  effectToTest.staminaDamage = 55;
                
                ProcessInstantEffect(effect);
            }
            
        }
    }
}

