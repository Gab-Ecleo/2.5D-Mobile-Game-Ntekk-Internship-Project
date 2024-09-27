using System;
using System.Collections.Generic;
using EventScripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class MapRandomizer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> maps;

        private void Start()
        {
            foreach (GameObject map in maps)
            {
                map.SetActive(false);
            }
            
            RandomizeMap();
        }

        private void RandomizeMap()
        {
            var index = Random.Range(0, 2);

            switch (index)
            {
                case 0:
                    maps[index].SetActive(true);
                    break;
                case 1:
                    maps[index].SetActive(true);
                    break;
            }
            
            AudioEvents.RANDOMIZE_AUDIO?.Invoke(index);
        }
    }
}

