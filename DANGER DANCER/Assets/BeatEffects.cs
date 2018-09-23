using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEffects : MonoBehaviour
{

    private SpriteEffects effects;

	// Use this for initialization
	void Start ()
    {
        effects = GetComponent<SpriteEffects>();
	    BeatManager.Instance.OnBeat += OnBeat;
	}
	
    void OnBeat()
    {
        effects.deformX = 0.05f;
        effects.deformY = 0.05f;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
