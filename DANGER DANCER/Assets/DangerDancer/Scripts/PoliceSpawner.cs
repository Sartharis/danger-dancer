using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSpawner : MonoBehaviour
{
    [SerializeField] int maxSpawned = 3;
    [SerializeField] float spawnDelay = 10.0f;
    [SerializeField] PlayerChaser spawnTarget;

    private int spawned = 0;
    private float spawnTime = 0;

	// Use this for initialization
	void Start () {
		
	}
	
    public void Spawn()
    {
        spawned += 1;
        Instantiate(spawnTarget, transform.position, transform.rotation);
    }

	// Update is called once per frame
	void Update ()
    {
		if( spawned < maxSpawned )
        {
            //spawnTime += Time.deltaTime;
            if(gameObject.GetComponent<EventManager>().CheckTag("Police"))
            {
                //spawnTime = 0;
                Spawn();
            }
        }
        //else
        //{
        //    spawnTime = 0;
        //}
	}
}
