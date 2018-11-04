using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public float moveSpeed= 3;
    private Vector2 moveDir;
    private bool inLight = false;
    Rigidbody2D rigidBody;
    private float time=0;


  void Start ()
    {
    rigidBody = GetComponent<Rigidbody2D>();
        moveDir = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        moveDir.Normalize();
  }
  private void ChangeDirection(Collision2D collision)
    {
        Vector2 normal_2d = collision.contacts[0].normal;
        Vector3 direction = rigidBody.velocity;
        Vector2 reflect = Vector2.Reflect(moveDir, normal_2d) + Random.insideUnitCircle;
        reflect.Normalize();
        moveDir = reflect;
        
    }

    private void Update()
    {
        // transform.Translate(0, 0, translation);
        float x_f = moveDir.x * moveSpeed * Time.deltaTime;
        float y_f = moveDir.y * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(rigidBody.position.x + x_f, rigidBody.position.y + y_f);
        if (inLight){
            time += Time.deltaTime;
            if (time > 2.0){
                ScoreManager.Instance.AddScore(15, "Spotlight", transform.position);
                time = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inLight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inLight = false;
        }

    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        ChangeDirection(coll);
    }
}
