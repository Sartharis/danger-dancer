using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyNav.PolyNavAgent))]
public class JumpingPlayerChaser : MonoBehaviour
{
    public float initSpeed= 3;
    private Vector2 moveDir;
    Rigidbody2D rigidBody;
    private PolyNav.PolyNavAgent navAgent; 
    private bool inArena = false;
    private float maxSpeed;
    private float speedModifier;
    private float accelerationModifier;
    private float maxMass;
    [SerializeField] private float minMass;
    [SerializeField] private float massReduceRadius;

	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        moveDir = GameObject.FindGameObjectsWithTag("Arena")[0].transform.position - transform.position;
        var angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        Debug.Log(angle);
        Debug.Log(Quaternion.AngleAxis(angle, transform.forward));
        transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
        moveDir.Normalize();
        
	}
	
	void Update ()
    {
        if (inArena)
        {
            Vector3 playerPos = FindObjectOfType<PlayerDancer>().transform.position;
            navAgent.SetDestination(playerPos);
            navAgent.maxSpeed = maxSpeed + speedModifier;
            speedModifier = Mathf.Lerp(speedModifier, 0, 0.01f);
            if((transform.position - playerPos).magnitude <= massReduceRadius)
            {
                navAgent.mass = (1 - ((massReduceRadius - (transform.position - playerPos).magnitude))/massReduceRadius) * (maxMass-minMass) + minMass;
            }
            else
            {
                navAgent.mass = maxMass;
            }
        }
        else
        {
            // transform.Translate(0, 0, translation);
            float x_f = moveDir.x * initSpeed * Time.deltaTime;
            float y_f = moveDir.y * initSpeed * Time.deltaTime;
            transform.position = new Vector2(rigidBody.position.x + x_f, rigidBody.position.y + y_f);
        }
	}

    public void ModifySpeed(float speedModifier)
    {
        speedModifier += speedModifier;
    }

    public void ModifyAcceleration(float accelerationMod)
    {
        accelerationModifier *= accelerationMod;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Transform other = collision.transform;
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerDancer>().Fall(new Vector2());
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Danger")
        {
            Destroy(gameObject);
        }
        if (collision.tag == "Arena")
        {
            Debug.Log("In!");
            inArena = true;
            gameObject.layer = 9;
            navAgent = GetComponent<PolyNav.PolyNavAgent>();
            // navAgent.SetDestination(GameObject.FindGameObjectsWithTag("Arena")[0].transform.position);
            // Debug.Log(GameObject.FindGameObjectsWithTag("Arena")[0].transform.position);
            maxSpeed = navAgent.maxSpeed + Random.Range(-0.1f,0);
            maxMass = navAgent.mass;
        }
    }
}
