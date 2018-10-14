using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : UnitySingleton<SpawnManager>
{

	private int beat;
	[SerializeField] Transform[] spawnPositions;
	[SerializeField] public List<SpawnEvent> spawnList;
    [SerializeField] DelayedSpawner delayedSpawner;
    private int spawnIndex;

    private void Start()
    {
        beat = 0;
        spawnList = null;
        spawnIndex = 0;
    }

    public void StartSpawning ()
	{
        beat = 0;
        spawnIndex = 0;
		BeatManager.Instance.OnBeat += incrementBeat;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (spawnList == null)
		{
			spawnList = XMLDeserializer.Instance.getSpawnEvent();
		}
	}

	void incrementBeat ()
	{
		beat += 1;
		spawnObjects ();
	}

	void spawnObjects ()
	{
		if (spawnList != null)
		{
			while (spawnIndex < spawnList.Count && beat >= spawnList [spawnIndex].beat)
			{
                DelayedSpawner dspawner = Instantiate(delayedSpawner, spawnPositions[spawnList[spawnIndex].index].position, Quaternion.identity);
                GameObject obj = GetComponent<SpawnDict>().get(spawnList[spawnIndex].spawned);
                dspawner.objectToSpawn = obj;
                spawnIndex += 1;
			}
		}
	}
}
