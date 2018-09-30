using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatShrink : MonoBehaviour {

    [SerializeField] private int beatsToShrink = 10;
    float scalar = 0;
    // Use this for initialization
    void Start () {
        scalar = -transform.localScale.x / beatsToShrink;
        BeatManager.Instance.OnBeat += OnBeat;
    }

    void OnBeat() {

        transform.localScale += new Vector3(scalar, scalar, 0);

        if (transform.localScale.x <= 0){
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
