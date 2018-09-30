using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingMine : MonoBehaviour
{
    public float moveSpeed= 3;
    private Vector2 moveDir;
    Rigidbody2D rigidBody;


	void Start ()
    {
		rigidBody = GetComponent<Rigidbody2D>();
        // rigidBody.velocity = transform.up * moveSpeed;
        // rigidBody.velocity = new Vector3(Random.Range(-1.0f, 1.0f),Random.Range(-1.0f, 1.0f),0);
        // rigidBody.velocity = rigidBody.velocity * moveSpeed;
        moveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        moveDir.Normalize();
        // moveDir = moveDir * moveSpeed;
	}
	private void ChangeDirection(Collision2D collision)
    {
        // Debug.Log(rigidBody.velocity);
        // rigidBody.velocity = new Vector3 (rigidBody.velocity.x,-rigidBody.velocity.y,0);
        // Vector3 normal = normal_2d;
        // Vector3 Vnew = -2*(Vector3.Dot(normal, direction))*normal + direction;
        // rigidBody.velocity = new Vector3(Random.Range(-1.0f, 1.0f),Random.Range(-1.0f, 1.0f),0);
        // rigidBody.velocity = rigidBody.velocity * moveSpeed;
        Vector2 normal_2d = collision.contacts[0].normal;
        Vector3 direction = rigidBody.velocity;
        Vector2 reflect = Vector2.Reflect(moveDir, normal_2d);
        reflect.Normalize();
        moveDir = reflect;
        
    }

    private void Update()
    {
        // Debug.Log(moveDir);
        float x_f = moveDir.x * moveSpeed * Time.deltaTime;
        float y_f = moveDir.y * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(rigidBody.position.x + x_f, rigidBody.position.y + y_f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hmm seems like this is the collision");
        Debug.Log(collision.tag);
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
        // Vector3 newV = new Vector3(-rigidBody.velocity.x, -rigidBody.velocity.y, 0);
        // newV.Normalize();
        // rigidBody.velocity = newV * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Am I getting here at all?");
        ChangeDirection(coll);
    }
}
