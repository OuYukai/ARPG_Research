using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

namespace SG
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        [SerializeField] PlayerManager player;

        [Header("SAVE / LOAD")] 
        [SerializeField] private bool saveGame;
        [SerializeField] private bool loadGame;
        
        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")] 
        private SaveFileDataWriter saveFileDataWriter;
        
        [Header("Current Character Data")] 
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;
        
        [Header("Character Slots")] 
        public CharacterSaveData characterSlot01;
        //public CharacterSaveData characterSlot02;
        //public CharacterSaveData characterSlot03;
        //public CharacterSaveData characterSlot04;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else 
                Destroy(gameObject);
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }

            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        private void DecideCharacterFileNameBasedOnCharacterSlotBeingUsed()
        {
            switch (currentCharacterSlotBeingUsed)
            {
                case CharacterSlot.CharacterSlot_01:
                    saveFileName = "CharacterSlot_01";
                    break;
                case CharacterSlot.CharacterSlot_02:
                    saveFileName = "CharacterSlot_02";
                    break;
                case CharacterSlot.CharacterSlot_03:
                    saveFileName = "CharacterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    saveFileName = "CharacterSlot_04";
                    break;
            }
        }

        public void CreateNewGame()
        {
            //  CREATE A NEW FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            currentCharacterData = new CharacterSaveData();
        }

        public void LoadGame()
        {
            //  LOAD A PREVIOUS FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();
            
            //  GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            //  SAVE THE CURRENT FILE UNDER A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            DecideCharacterFileNameBasedOnCharacterSlotBeingUsed();

            saveFileDataWriter = new SaveFileDataWriter();
            //  GENERALLY WORKS ON MULTIPLE MACHINE TYPES (Application.persistentDataPath)
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            
            //  PASS THE PLAYERS INFO, FROM GAME, TO THEIR SAVE FILE
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);
            
            //  WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }
    
        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetworldSceneIndex()
        {
            return worldSceneIndex;
        }
    }
}

