using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCameraEffects : MonoBehaviour {

    [SerializeField] AnimationCurve filterCurve;
    [SerializeField] float filterPlayerFall;
    private AudioLowPassFilter audioFilter;
    private PlayerDancer player;

	// Use this for initialization
	void Start () {
		audioFilter = GetComponent<AudioLowPassFilter>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDancer>();
	}

    // Update is called once per frame
    void Update()
    {
        if (player.isFallen())
        {
            audioFilter.cutoffFrequency = filterPlayerFall;
        }
        else
        {
            audioFilter.cutoffFrequency = filterCurve.Evaluate(ScoreManager.Instance.GetScoreRatio());
        }
    }
}
