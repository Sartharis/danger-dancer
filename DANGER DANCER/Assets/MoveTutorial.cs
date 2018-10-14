using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		GameObject obj = GameObject.FindGameObjectWithTag("Player");
        PlayerDancer dancer = obj.GetComponent<PlayerDancer>();
        if(dancer && dancer.actionState == EActionState.AS_SPIN)
        {
            TutorialManager.Instance.NextPhase();
            Destroy(gameObject);
        }
	}
}
