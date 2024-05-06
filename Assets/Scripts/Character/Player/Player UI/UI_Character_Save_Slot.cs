using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SG
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        SaveFileDataWriter saveFileDataWriter;

        [Header("Game Slot")] 
        public CharacterSlot characterSlot;

        [Header("Character Info")] 
        public TextMeshProUGUI characterName;

        public TextMeshProUGUI timePlayed;

        private void OnEnable()
        {
            LoadSaveSlot();
        }

        private void LoadSaveSlot()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            
            // SAVE SLOT 01
            if (characterSlot == CharacterSlot.CharacterSlot_01)
            {
                saveFileDataWriter.saveFileName =
                    WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                //  IF THE FILE EXISTS, GET INFORMATION FORM IT
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                }
                //  IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 02
            else if (characterSlot == CharacterSlot.CharacterSlot_02)
            {
                saveFileDataWriter.saveFileName =
                    WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                //  IF THE FILE EXISTS, GET INFORMATION FORM IT
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                }
                //  IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 03
            else if (characterSlot == CharacterSlot.CharacterSlot_03)
            {
                saveFileDataWriter.saveFileName =
                    WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                //  IF THE FILE EXISTS, GET INFORMATION FORM IT
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                }
                //  IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }
            // SAVE SLOT 04
            else if (characterSlot == CharacterSlot.CharacterSlot_04)
            {
                saveFileDataWriter.saveFileName =
                    WorldSaveGameManager.instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot);
                
                //  IF THE FILE EXISTS, GET INFORMATION FORM IT
                if (saveFileDataWriter.CheckToSeeIfFileExists())
                {
                    characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                }
                //  IF IT DOES NOT, DISABLE THIS GAMEOBJECT
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreanManager.instance.SelectCharacterSlot(characterSlot);
        }
    }
}

