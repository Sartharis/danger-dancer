using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBeatModify : MonoBehaviour {

    MeshRenderer meshrend;

	// Use this for initialization
	void Start ()
    {
	    BeatManager.Instance.OnBeat += OnBeat;	
        meshrend = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnBeat()
    {
        Color col = Color.HSVToRGB(Random.Range(0.0f, 1.0f),1,1);
        meshrend.material.color = col;
    }
}
