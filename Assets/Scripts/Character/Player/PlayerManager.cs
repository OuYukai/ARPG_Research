using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

namespace SG
{
    public class PlayerManager : CharacterManager
    {
        [Header("DEBUG MENU")] 
        [SerializeField] private bool respwnCharacter = false;
        //[SerializeField] private bool switchRightWeapon = false;
        
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerNetworkManager playerNetworkManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;

        protected override void Awake()
        {
            base.Awake();

            // DO MORE STUFF, ONLY FOR THE PLAYER
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerNetworkManager = GetComponent<PlayerNetworkManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
        }

        protected override void Update()
        {
            base.Update();

            // IF WE DO NOT OWN THIS GAMEOBJECTM WE DO NOT CONTROL OR EDIT IT
            if (!IsOwner)
            {
                return;
            }

            // HANDLE MOVEMENT
            playerLocomotionManager.HandleAllMovement();

            //  REGENERATE STAMINA
            playerStatsManager.RegenerateStamina();
            
            DebugMenu();
        }

        protected override void LateUpdate()
        {
            if (!IsOwner)
            {
                return;
            }

            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraAction();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;

            //  IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
            if (IsOwner)
            {
                PlayerCamera.instance.player = this;
                PlayerInputManager.instance.player = this;
                WorldSaveGameManager.instance.player = this;
                
                //  UPDATE THE TOTAL AMOUNT OF HEALTH OR STAMINA WHEN THE STAT LIKED TO EITHER CHANGES
                playerNetworkManager.vitality.OnValueChanged += playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged += playerNetworkManager.SetNewMaxStaminaValue;
                
                //  UPDATES UI STAT BASE WHEN A STAT CHANGES (HEALTH OR STAMINA)
                playerNetworkManager.currentHealth.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged += playerStatsManager.ResetStaminaRegenerationTimer;
            }
            
            //  STATS
            playerNetworkManager.currentHealth.OnValueChanged += playerNetworkManager.CheckHP;
            
            //  LOCK ON
            playerNetworkManager.isLockedOn.OnValueChanged += playerNetworkManager.OnIsLockedOnChange;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged += playerNetworkManager.OnLockOnTargetIDChange;
            
            //  EQUIPMENT
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged += playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged += playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            
            //  FLAGS
            playerNetworkManager.isChargingAttack.OnValueChanged += playerNetworkManager.OnIsChargingAttackChanged;
            
            //  UPON CONNECTING, IF WE ARE THE OWNER OF THIS CHARACTER, BUT WE ARE NOT THE SERVER, RELOAD OUR CHARACTER DATA TO THIS NEWLY INSTANTIATED CHARACTER
            //  WE DON'T RUN THIS IF WE ARE THE SERVER, BECAUSE SINCE THEY ARE THE HOST, THEY ARE ALREADY LOADED IN AND DON'T NEED TO RELOAD THEIR DATA
            if (IsOwner && !IsServer)
            {
                LoadGameDataFromCurrentCharacterData(ref WorldSaveGameManager.instance.currentCharacterData);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;

            //  IF THIS IS THE PLAYER OBJECT OWNED BY THIS CLIENT
            if (IsOwner)
            {
                //  UPDATE THE TOTAL AMOUNT OF HEALTH OR STAMINA WHEN THE STAT LIKED TO EITHER CHANGES
                playerNetworkManager.vitality.OnValueChanged -= playerNetworkManager.SetNewMaxHealthValue;
                playerNetworkManager.endurance.OnValueChanged -= playerNetworkManager.SetNewMaxStaminaValue;
                
                //  UPDATES UI STAT BASE WHEN A STAT CHANGES (HEALTH OR STAMINA)
                playerNetworkManager.currentHealth.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue;
                playerNetworkManager.currentStamina.OnValueChanged -= PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue;
                playerNetworkManager.currentStamina.OnValueChanged -= playerStatsManager.ResetStaminaRegenerationTimer;
            }
            
            //  STATS
            playerNetworkManager.currentHealth.OnValueChanged -= playerNetworkManager.CheckHP;
            
            //  LOCK ON
            playerNetworkManager.isLockedOn.OnValueChanged -= playerNetworkManager.OnIsLockedOnChange;
            playerNetworkManager.currentTargetNetworkObjectID.OnValueChanged -= playerNetworkManager.OnLockOnTargetIDChange;
            
            //  EQUIPMENT
            playerNetworkManager.currentWeaponBeingUsed.OnValueChanged -= playerNetworkManager.OnCurrentWeaponBeingUsedIDChange;
            playerNetworkManager.currentRightHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentRightHandWeaponIDChange;
            playerNetworkManager.currentLeftHandWeaponID.OnValueChanged -= playerNetworkManager.OnCurrentLeftHandWeaponIDChange;
            
            //  FLAGS
            playerNetworkManager.isChargingAttack.OnValueChanged -= playerNetworkManager.OnIsChargingAttackChanged;
        }

        private void OnClientConnectedCallback(ulong clientID)
        {
            //  KEEP A LIST OF ACTIVE PLAYERS IN THE GAME
            WorldGameSessionManager.instance.AddPlayerToActivePlayersList(this);
            
            //  IF WE ARE THE SERVER, WE ARE THE HOST, SO WE DON'T NEED TO LOAD PLAYERS TO SYNC THEM
            //  YOU ONLY NEED TO LOAD OTHER PLAYERS GEAR TO SYNC IT IF YOU JOIN A GAME THAT ALREADY BEEN ACTIVE WITHOUT YOU BEING PRESENT
            if (!IsServer && IsOwner)
            {
                foreach (var player in WorldGameSessionManager.instance.players)
                {
                    if (player != this)
                    {
                        player.LoadOtherPlayerCharacterWhenJoiningServer();
                    }
                }
            }
        }
        
        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            if (IsOwner)
            {
                PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();
            }
            
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
            
            //  CHECK FOR PLAYERS THAT ARE ALIVE, IF 0 RESPAWN CHARACTERS
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            if (IsOwner)
            {
                isDead.Value = false;
                playerNetworkManager.currentHealth.Value = playerNetworkManager.maxHealth.Value;
                playerNetworkManager.currentStamina.Value = playerNetworkManager.maxStamina.Value;
                //  RESTORE FOCUS POINTS
                
                //  PLAY REBIRTH EFFECTS
                playerAnimatorManager.PlayerTargetActionAnimation("Empty", false);
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = playerNetworkManager.characterName.Value.ToString();
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;
            
            currentCharacterData.currentHealth = playerNetworkManager.currentHealth.Value;
            currentCharacterData.currentStamina = playerNetworkManager.currentStamina.Value; 

            currentCharacterData.vitality = playerNetworkManager.vitality.Value;
            currentCharacterData.endurance = playerNetworkManager.endurance.Value;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            playerNetworkManager.characterName.Value = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition,
                currentCharacterData.zPosition);
            transform.position = myPosition;
            
            playerNetworkManager.vitality.Value = currentCharacterData.vitality;
            playerNetworkManager.endurance.Value = currentCharacterData.endurance;
            
            //  THIS WILL BE MOVED WHEN SAVING AND LOADING IS ADDED
            playerNetworkManager.maxHealth.Value =
                playerStatsManager.CalculateHealthBasedOnVitalityLevel(playerNetworkManager.vitality.Value);
            playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            playerNetworkManager.currentHealth.Value = currentCharacterData.currentHealth;
            playerNetworkManager.currentStamina.Value = currentCharacterData.currentStamina;
            PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerNetworkManager.maxStamina.Value);
        }

        public void LoadOtherPlayerCharacterWhenJoiningServer()
        {
            //  SYNC WEAPONS
            playerNetworkManager.OnCurrentRightHandWeaponIDChange(0, playerNetworkManager.currentRightHandWeaponID.Value);
            playerNetworkManager.OnCurrentLeftHandWeaponIDChange(0, playerNetworkManager.currentLeftHandWeaponID.Value);
            
            //  ARMOR
            
            //  LOCK ON
            if (playerNetworkManager.isLockedOn.Value)
            {
                playerNetworkManager.OnLockOnTargetIDChange(0, playerNetworkManager.currentTargetNetworkObjectID.Value);
            }
        }
        
        //  DEBUG DELETE LATER
        private void DebugMenu()
        {
            if (respwnCharacter)
            {
                respwnCharacter = false;
                ReviveCharacter();
            }

            //if (switchRightWeapon)
            //{
            //    switchRightWeapon = false;
            //    playerEquipmentManager.SwitchRightWeapon();
            //}
        }
    }
}

