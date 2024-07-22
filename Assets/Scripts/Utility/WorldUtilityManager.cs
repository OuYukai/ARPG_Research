using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


namespace SG
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager instance;

        [Header("Layers")] 
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask enviroLayers;
        
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

        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviroLayerMask()
        {
            return enviroLayers;
        }
    }
}

