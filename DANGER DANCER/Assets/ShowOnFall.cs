using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnFall : MonoBehaviour
{

    MeshRenderer mesh;
    PlayerDancer player;

	// Use this for initialization
	void Start ()
    {
        mesh = GetComponent<MeshRenderer>();
		player = GetComponentInParent<PlayerDancer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		mesh.enabled = player.isFallen();
        foreach(MeshRenderer i in GetComponentsInChildren<MeshRenderer>())
        {
            i.enabled = player.isFallen();
        }
	}
}
