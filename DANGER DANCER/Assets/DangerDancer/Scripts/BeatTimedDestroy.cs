using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTimedDestroy : MonoBehaviour
{
    public float beatsLeft = 8;

    void Start()
    {
        BeatManager.Instance.OnBeat += BeatHit;
    }
	
	// Update is called once per frame
	void BeatHit ()
    {
		beatsLeft -= 1;
        if(beatsLeft <= 0)
        {
            Destroy(gameObject);
        }
	}

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= BeatHit;
    }
}
