using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRingTutorial : MonoBehaviour {

    private bool finished = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!finished)
        {
            SpinRing[] spinrings = GetComponentsInChildren<SpinRing>();
            bool allCompleted = true;
            for (int i = 0; i < spinrings.Length; i++)
            {
                if (!spinrings[i].spin)
                {
                    allCompleted = false;
                    break;
                }
            }

            if (allCompleted)
            {
                finished = true;
                GetComponentInChildren<MeshRenderer>().enabled = false;
                TutorialManager.Instance.NextPhase();
                Destroy(gameObject);
            }
        }
    }
}
