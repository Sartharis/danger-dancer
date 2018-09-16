using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolyNav.PolyNavAgent))]
public class PlayerChaser : MonoBehaviour
{
    private PolyNav.PolyNavAgent navAgent; 
    private float maxSpeed;
    private float speedModifier;
    private float accelerationModifier;
    private float maxMass;
    [SerializeField] private float minMass;
    [SerializeField] private float massReduceRadius;

	void Start ()
    {
		navAgent = GetComponent<PolyNav.PolyNavAgent>();
        navAgent.SetDestination(FindObjectOfType<PlayerDancer>().transform.position);
        maxSpeed = navAgent.maxSpeed + Random.Range(-0.1f,0);
        maxMass = navAgent.mass;
	}
	
	void Update ()
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
    }
}
