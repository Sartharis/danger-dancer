using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpawnEntry
{
    public string key;
    public GameObject spawnObj;
    public bool isBad;
};

public class SpawnDict : UnitySingleton<SpawnDict>
{
    [SerializeField] List<SpawnEntry> spawnEntry;

    public SpawnEntry get(string k)
    {
        for (int i = 0; i < spawnEntry.Count; i++)
        {
            if (spawnEntry[i].key == k)
            {
                return spawnEntry[i];
            }
        }

        return spawnEntry[0];
    }
}
