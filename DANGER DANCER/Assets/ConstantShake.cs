using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantShake : MonoBehaviour {

    [SerializeField] float shakeValue;
    Shaker shake;

	// Use this for initialization
	void Start () {
		shake = GetComponent<Shaker>();
	}
	
	// Update is called once per frame
	void Update () {
		shake.shake = shakeValue;
	}
}
