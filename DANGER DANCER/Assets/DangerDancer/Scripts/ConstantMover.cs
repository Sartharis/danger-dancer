using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantMover : MonoBehaviour
{
    public float moveSpeed= 5;
    Rigidbody2D rigidBody;

	void Start ()
    {
		rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = transform.up * moveSpeed;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
