using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCameraEffects : MonoBehaviour {

    [SerializeField] AnimationCurve filterCurve;
    private AudioLowPassFilter audioFilter;

	// Use this for initialization
	void Start () {
		audioFilter = GetComponent<AudioLowPassFilter>();
	}
	
	// Update is called once per frame
	void Update () {
		audioFilter.cutoffFrequency = filterCurve.Evaluate(ScoreManager.Instance.GetScoreRatio());
	}
}
