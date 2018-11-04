using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrowIn : MonoBehaviour {

  public string type;
  private float moveSpeed = 7;
  private Vector2 moveDir;
  private Vector3 targetPos;
  Rigidbody2D rigidBody;
	// Set target
	void Start () {
    rigidBody = GetComponent<Rigidbody2D>();
    if (type == "player"){
      Vector3 playerPos = FindObjectOfType<PlayerDancer>().transform.position;
      targetPos = playerPos + (playerPos - transform.position) * 2;
    }
    else {
      targetPos = GameObject.FindGameObjectsWithTag("Arena")[0].transform.position;
    }
		moveDir = targetPos - transform.position;
    moveDir.Normalize();
	}
	
  void arrived () {
    Vector2 tp = targetPos;
    Vector2 mp = transform.position;
    if ((tp - mp).sqrMagnitude < 0.1){
        Destroy(gameObject);
    }
  }

	// Move towards the direction
	void Update () {
    float x_f = moveDir.x * moveSpeed * Time.deltaTime;
    float y_f = moveDir.y * moveSpeed * Time.deltaTime;
    transform.position = new Vector2(rigidBody.position.x + x_f, rigidBody.position.y + y_f);
    transform.Rotate (0,0,480*Time.deltaTime); //rotates 50 degrees per second around z axis
    arrived();
	}

  // If hitting the player destroy it
  private void OnTriggerEnter2D(Collider2D collision){

    Transform other = collision.transform;
    if (collision.tag == "Player")
    {
        other.GetComponent<PlayerDancer>().Fall(new Vector2());
        Destroy(gameObject);
    }
  }
}
