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
    private bool Fallen = false;
    private TimedDestroy destroyTime;
    private float maxSpeed;
    private float speedModifier;
    private float accelerationModifier;
    private float maxMass;
    private SpriteRenderer spriteRenderer;
    private Animator anim;


    [SerializeField] private float minMass;
    [SerializeField] private float massReduceRadius;

	void Start ()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        destroyTime = GetComponent<TimedDestroy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveDir = GameObject.FindGameObjectsWithTag("Arena")[0].transform.position - transform.position;
        moveDir.Normalize();
        
	}
	
	void Update ()
    {
        if (inArena)
        {
            anim.enabled = true;

            if (destroyTime.timeLeft<.75){
                anim.SetBool("Fallen", true);
                rigidBody.velocity = navAgent.currentSpeed*navAgent.movingDirection;
                navAgent.enabled = false;

            }
            else{

                Vector3 playerPos = FindObjectOfType<PlayerDancer>().transform.position;
                navAgent.SetDestination(playerPos);
                navAgent.maxSpeed = maxSpeed + speedModifier;
                speedModifier = Mathf.Lerp(speedModifier, 0, 0.01f);
                if ((transform.position - playerPos).magnitude <= massReduceRadius)
                {
                    navAgent.mass = (1 - ((massReduceRadius - (transform.position - playerPos).magnitude)) / massReduceRadius) * (maxMass - minMass) + minMass;
                }
                else
                {
                    navAgent.mass = maxMass;
                }
                if ((playerPos - transform.position).x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }

        }
        else
        {
            anim.enabled = false;
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
            inArena = true;
            gameObject.layer = 9;
            navAgent = GetComponent<PolyNav.PolyNavAgent>();
            maxSpeed = navAgent.maxSpeed + Random.Range(-0.1f,0);
            maxMass = navAgent.mass;
        }
    }
}
