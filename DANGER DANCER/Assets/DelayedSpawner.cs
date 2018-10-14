using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSpawner : MonoBehaviour {
    [SerializeField] private int beatsToShrink = 2;
    public GameObject objectToSpawn;
    float scalar = 0;
    // Use this for initialization
    void Start()
    {
        scalar = -transform.localScale.x / beatsToShrink;
        BeatManager.Instance.OnBeat += OnBeat;
    }

    void OnBeat()
    {

        transform.localScale += new Vector3(scalar, scalar, 0);

        if (transform.localScale.x <= 0)
        {
            BeatManager.Instance.OnBeat -= OnBeat;
            if (objectToSpawn){
                Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    // Use this for initialization
	
	// Update is called once per frame
	void Update () {
		
	}
}
