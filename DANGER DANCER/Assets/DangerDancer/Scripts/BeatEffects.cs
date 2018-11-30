using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatEffects : MonoBehaviour
{

    private SpriteEffects effects;
    public float pulseIntensity = 0.05f;

	// Use this for initialization
	void Start ()
    {
        effects = GetComponent<SpriteEffects>();
	    BeatManager.Instance.OnBeat += OnBeatB;
	}
	
    void OnBeatB()
    {
        effects.deformX += pulseIntensity;
        effects.deformY += pulseIntensity;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
