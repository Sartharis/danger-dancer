using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileEntry
{
    public string key;
    public GameObject spawnObj;
};

public class TileDict : MonoBehaviour
{
    [SerializeField] List<SpawnEntry> spawnEntry;

    public GameObject get(string k)
    {
        for (int i = 0; i < spawnEntry.Count; i++)
        {
            if (spawnEntry[i].key == k)
            {
                return spawnEntry[i].spawnObj;
            }
        }

        return null;
    }
}

