using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildingSystem
{
    [Serializable]
    public class Buildable
    {

        [field: SerializeField]
        public BuildableItem BuildableType { get; private set; }

        [field: SerializeField]
        public GameObject GameObject { get; private set; }

        [field: SerializeField]
        public Vector3Int Coordinates { get; private set; } 

        public Buildable(BuildableItem type, Vector3Int coords, GameObject gameObject = null)
        {
            BuildableType = type;
            Coordinates = coords;
            GameObject = gameObject;
        }

        public void Destroy()
        {
            if (GameObject != null)
            {
                UnityEngine.Object.Destroy(GameObject);
            }
        }
    }
}

