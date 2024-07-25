using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AIState : ScriptableObject
    {
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {
            
            //  DO SOME LOGIC TO FIND THE PLAYER
            //  IF WE HAVE FOUND THE PLAYER, RETURN THE PURSUE TARGET STATE INSTEAD
            //  IF WE HAVE NOT FOUND THE PLAYER, CONTINUE TO RETURN THE IDLE STATE
            
            return this;
        }
    }
}

