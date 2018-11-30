using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingMine : MonoBehaviour
{
    public float moveSpeed= 3;
    public bool isRandomDir = true;
    public Vector2 initialDir = new Vector2(1,0); 
    public Vector2 moveDir;
    Rigidbody2D rigidBody;
    ParticleSystem effect;


	void Start ()
    {
		rigidBody = GetComponent<Rigidbody2D>();
        effect = GetComponent<ParticleSystem>();
        if( isRandomDir)
        {
            moveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));    
        }
        else
        {
            moveDir = initialDir;
        }

        moveDir.Normalize();
    }

	private void ChangeDirection(Collision2D collision)
    {
        Vector2 normal_2d = collision.contacts[0].normal;
        Vector3 direction = rigidBody.velocity;
        Vector2 reflect = Vector2.Reflect(moveDir, normal_2d);
        reflect.Normalize();
        moveDir = reflect;
    }

    private void Update()
    {
        float x_f = moveDir.x * moveSpeed * Time.deltaTime;
        float y_f = moveDir.y * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(rigidBody.position.x + x_f, rigidBody.position.y + y_f);
        transform.rotation = Quaternion.Euler(0,0,180/Mathf.PI * Mathf.Atan2(moveDir.y,moveDir.x));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        effect.Play();
        ChangeDirection(coll);
    }
}
