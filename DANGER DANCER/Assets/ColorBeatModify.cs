using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBeatModify : MonoBehaviour {

    MeshRenderer meshrend;

	// Use this for initialization
	void Start ()
    {
	    BeatManager.Instance.OnBeat += OnBeat;	
        meshrend = GetComponent<MeshRenderer>();
        OnBeat();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }

    void OnBeat()
    {
        //if(LevelManager.Instance.levelStarted)
      //  {

            Color col = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 1f, 0.6f);
            meshrend.material.color = col;
     //   }
    }
}
