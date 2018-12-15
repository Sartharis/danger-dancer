using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDialogue : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.Instance.sawDialogue.Clear();
        GameManager.Instance.didTutorial = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
