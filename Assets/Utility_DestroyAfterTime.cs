using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class Utility_DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float timeUntilDestroyed = 5;


        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}

