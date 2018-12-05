using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SpawnManager : UnitySingleton<SpawnManager>
{

	[SerializeField] public Transform[] spawnPositions;
	[SerializeField] public List<SpawnEvent> spawnList;
    [SerializeField] DelayedSpawner delayedSpawner;
    [SerializeField] public int numDelay = 2;
    public TextAsset spawnData;
    private int spawnIndex;

    private void Start()
    {
        spawnList = null;
        spawnIndex = 0;
    }

    public void StartSpawning ()
	{
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
		spawnObjects ();
	}

	void spawnObjects ()
	{
		if (spawnList != null)
		{
            //Moving up to the correct beat index
            while (spawnIndex < spawnList.Count && BeatManager.Instance.getCurrentBeat() > spawnList[spawnIndex].beat - numDelay)
            {
                spawnIndex += 1;
            }
            //Moving down to the correct beat index
            while (spawnIndex < spawnList.Count && spawnIndex > 0 && BeatManager.Instance.getCurrentBeat() < spawnList[spawnIndex].beat - numDelay)
            {
                spawnIndex -= 1;
            }
            //Actually spawning stuff
            while (spawnIndex < spawnList.Count && BeatManager.Instance.getCurrentBeat() == spawnList [spawnIndex].beat - numDelay)
			{
                DelayedSpawner dspawner = Instantiate(delayedSpawner, spawnPositions[spawnList[spawnIndex].index].position, Quaternion.identity);
                GameObject obj = SpawnDict.Instance.get(spawnList[spawnIndex].spawned).spawnObj;
                dspawner.isBad = SpawnDict.Instance.get(spawnList[spawnIndex].spawned).isBad;
                dspawner.objectToSpawn = obj;
                spawnIndex += 1;
                
			}
		}
	}
}
