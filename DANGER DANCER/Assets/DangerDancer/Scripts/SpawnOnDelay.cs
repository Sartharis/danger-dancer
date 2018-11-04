using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDelay : MonoBehaviour
{
    [SerializeField] private float delay;

	// Use this for initialization
	void Start ()
    {
        MonoBehaviour[] cs = GetComponentsInChildren<MonoBehaviour>();
        for (int i = 0; i < cs.Length; i++)
        {
            cs[i].enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
		delay -= Time.deltaTime;
        if(delay <= 0.0f)
        {
            MonoBehaviour[] cs = GetComponentsInChildren<MonoBehaviour>();
            for( int i = 0; i < cs.Length; i++)
            {
                cs[i].enabled = true;
            }
        }
	}
}
