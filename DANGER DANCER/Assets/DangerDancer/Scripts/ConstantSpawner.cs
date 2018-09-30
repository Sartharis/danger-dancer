using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSpawner : MonoBehaviour
{
    [SerializeField] float spawnDelay = 10.0f;
    [SerializeField] Transform spawnTarget;
    [SerializeField] bool startSpawning = true;

    private int spawned = 0;
    private float spawnTime = 0;

	void Start ()
    {
		if(startSpawning)
        {
            spawnTime = spawnDelay;
        }
	}
	
    public void Spawn()
    {
        spawned += 1;
        Instantiate(spawnTarget, transform.position, transform.rotation);
    }
    void Update()
    {
        //if (spawned < maxSpawned)
        //{
            //spawnTime += Time.deltaTime;
            if (GameObject.Find("LevelManager").GetComponent<EventManager>().CheckTag("Police"))
            {
                //spawnTime = 0;
                Spawn();
            }
        //}
        //else
        //{
        //    spawnTime = 0;
        //}
    }
    //void Update ()
    //{
    //    spawnTime += Time.deltaTime;
    //    if (spawnTime >= spawnDelay)
    //    {
    //        spawnTime = 0;
    //        Spawn();
    //    }
    //}
}
