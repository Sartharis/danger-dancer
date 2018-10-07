using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : UnitySingleton<SpawnManager>
{

	public int beat;
	[SerializeField] Transform[] spawnPositions;
	public List<SpawnEvent> spawnList;
	public int spawnIndex;

	// Use this for initialization
	void Start ()
	{
		beat = 0;
		BeatManager.Instance.OnBeat += incrementBeat;
		spawnIndex = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (spawnList == null)
		{
			spawnList = GetComponent<XMLDeserializer> ().getSpawnEvent ();
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
				GameObject obj = GetComponent<SpawnDict>().get (spawnList [spawnIndex].spawned);
				Instantiate (obj, spawnPositions [spawnList [spawnIndex].index].position, Quaternion.identity);
				spawnIndex += 1;
			}
		}
	}
}
