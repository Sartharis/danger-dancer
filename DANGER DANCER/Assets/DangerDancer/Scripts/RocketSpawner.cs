using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour {

	[SerializeField] float spawnDelay = 3.0f;
	[SerializeField] float rocketSpeed = 5.0f;
	[SerializeField] ConstantMover spawnTarget;

	private float spawnTime = 0;

	// Use this for initialization
	void Start ()
	{
		spawnTime = spawnDelay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Transform player = FindObjectOfType<PlayerDancer> ().transform;
		Vector3 toPlayer = transform.position - player.position;
		Vector3 rotate = Vector3.RotateTowards (transform.forward, toPlayer, 1, 0);
		Quaternion rotation = Quaternion.LookRotation (rotate, Vector3.forward);
		rotation.x = 0;
		rotation.y = 0;
		transform.rotation = rotation;
		if (spawnTime >= spawnDelay)
		{
			ConstantMover r = Instantiate (spawnTarget, transform.position, transform.rotation);
			r.moveSpeed = rocketSpeed;
			spawnTime = 0;
		}
		spawnTime += Time.deltaTime;
	}
}
