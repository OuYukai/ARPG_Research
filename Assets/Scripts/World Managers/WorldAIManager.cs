using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace SG
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("DEBUG MENU")] 
        [SerializeField] private bool despawnCharacters = false;
        [SerializeField] private bool respawnCharacters = false;

        [Header("Characters")] 
        [SerializeField] private GameObject[] aiCharacters;
        [SerializeField] private List<GameObject> spawnedInCharacters;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                //  SPAWN ALL A.I IN SCENE
                StartCoroutine(WaitForSceneToLoadTheSpawnCharacters());
            }
        }

        private void Update()
        {
            if (despawnCharacters)
            {
                despawnCharacters = false;
                DespawnAllCharacters();
            }

            if (respawnCharacters)
            {
                respawnCharacters = false;
                SpawnAllCharacters();
            }
        }

        private IEnumerator WaitForSceneToLoadTheSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null;
            }
            
            SpawnAllCharacters();
        }

        private void SpawnAllCharacters()
        {
            foreach (var character in aiCharacters)
            {
                GameObject instantiatedCharacter = Instantiate(character);
                instantiatedCharacter.GetComponent<NetworkObject>().Spawn();
                spawnedInCharacters.Add(instantiatedCharacter);
            }
        }

        private void DespawnAllCharacters()
        {
            foreach (var character in spawnedInCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void DisableAllCharacters()
        {
            //  TODO : DISABLE CHARACTER GAME OBJECTS, SYNC DISABLE STATUS ON NETWORK
            //  TODO : DISABLE GAME OBJECTS FOR CLIENTS UPON CONNECTING, IF DISABLED STATUS IS TRUE
            //  TODO : CAN BE USED TO DISABLE CHARACTERS THAT ARE FAR FROM PLAYERS TO SAVE MEMORY
            //  TODO : CHARACTERS CAN BE SPLIT INTO AREAS (AREA_00, AREA_01, AREA_02) ECT
        }
    }
}

