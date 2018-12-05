using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRoundTutorial : MonoBehaviour {

    CircleRound c;
    bool complete = false;

	// Use this for initialization
	void Start ()
    {
		c = GetComponentInChildren<CircleRound>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(!c.activated && !complete)
        {
            complete = true;
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
            TutorialManager.Instance.NextPhase();
        }
	}
}
