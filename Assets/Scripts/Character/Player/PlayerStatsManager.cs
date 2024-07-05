using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        private PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Start()
        {
            base.Start();
            
            //  WHY CALCULATE THESE HERE?
            //  WHEN WE MAKE A CHARACTER CREATION MENU, AND SET THE STATS DEPENDING ON THE CLASS, THIS WILL BE CALCULATED THERE
            //  UNTIL THEN HOWEVER, STATS ARE NEVER CALCULATE, SO WE DO IT HERE ON START, IF A SAVE FILE EXISTS THEY WILL BE OVER WRITTEN WHEN LOADING INTO A SCENE
            CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        }
    }
}

