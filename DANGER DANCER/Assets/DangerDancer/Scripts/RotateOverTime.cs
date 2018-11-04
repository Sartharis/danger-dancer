using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    [SerializeField] private float rotSpeed = 5f;
	
	// Update is called once per frame
	void Update ()
    {
	    transform.Rotate(new Vector3(0,0,rotSpeed * Time.deltaTime));	
	}
}
