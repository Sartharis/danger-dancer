using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleByScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float scoreFactor = (ScoreManager.Instance.audienceScore / ScoreManager.Instance.audienceScoreMax );
		transform.localScale = new Vector3(scoreFactor, scoreFactor, scoreFactor);
	}
}
