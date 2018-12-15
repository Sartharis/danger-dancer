using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorial : MonoBehaviour
{
    [SerializeField] float spinCount = 2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		GameObject obj = GameObject.FindGameObjectWithTag("Player");
        PlayerDancer dancer = obj.GetComponent<PlayerDancer>();
        if(dancer && dancer.actionState == EActionState.AS_SPIN && spinCount <= 0)
        {
            TutorialManager.Instance.NextPhase();
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
            Destroy(gameObject);
        }

        spinCount -= Time.deltaTime;
	}
}
