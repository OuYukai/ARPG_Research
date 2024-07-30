﻿using System;
using UnityEngine;
using Unity.Netcode;

namespace SG
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")] 
        [SerializeField] GameObject characterGameObject;
        [SerializeField] GameObject instantiatedGameObject;

        private void Awake()
        {
            
        }

        private void Start()
        {
            WorldAIManager.instance.SpawnCharacter(this);
            gameObject.SetActive(false);
        }

        public void AttempToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject);
                instantiatedGameObject.transform.position = transform.position;
                instantiatedGameObject.transform.rotation = transform.rotation;
                instantiatedGameObject.GetComponent<NetworkObject>().Spawn();
            }
        }
        
    }
}