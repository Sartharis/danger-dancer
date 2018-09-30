using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceBar : MonoBehaviour {

    [SerializeField] float MinBarSize; 
    [SerializeField] float MaxBarSize;
    private RectTransform rect;

	// Use this for initialization
	void Start ()
    {
	    rect = GetComponent<RectTransform>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 10, MaxBarSize * ScoreManager.Instance.audienceScore / ScoreManager.Instance.audienceScoreMax);
	}
}
