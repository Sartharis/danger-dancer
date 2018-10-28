using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crowd : MonoBehaviour {

    [SerializeField] private AnimationCurve crowdShakeCurve;
    [SerializeField] private AnimationCurve crowdPulseCurve;
    private ConstantShake cShake;
    private BeatEffects bEffects;

	// Use this for initialization
	void Start () {
		bEffects = GetComponent<BeatEffects>();
        cShake = GetComponent<ConstantShake>();
	}
	
	// Update is called once per frame
	void Update () {
		bEffects.pulseIntensity = crowdPulseCurve.Evaluate(ScoreManager.Instance.GetScoreRatio());
        cShake.shakeValue = crowdShakeCurve.Evaluate(ScoreManager.Instance.GetScoreRatio());
	}
}
