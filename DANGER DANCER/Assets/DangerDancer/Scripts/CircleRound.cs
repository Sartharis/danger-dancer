 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRound : MonoBehaviour {
    Vector3 playerStart;
    [SerializeField] private float destroyTime = 0.5f;
    Vector3 playerPos;
    float angle;
    bool fullCircle = false;
    float prevAngle;
    bool activated=true;
    // Use this for initialization
    void Start () {
        prevAngle = 0;
        playerStart = transform.position;

    }

    void checkAngle(){
        angle = Vector2.SignedAngle(new Vector2(playerPos.x, playerPos.y), new Vector2(playerStart.x, playerStart.y) );
        float deltaAngle = angle - prevAngle;
        if(Mathf.Abs(deltaAngle)>350){
            fullCircle = !fullCircle;
        }
        if(fullCircle && Mathf.Abs(angle)<=5){
            ScoreManager.Instance.AddScore(20, "Circle Round", transform.position);
            activated = false;
        }
        prevAngle = angle;
    }
	// Update is called once per frame
	void Update () {
        playerPos = FindObjectOfType<PlayerDancer>().transform.position;
        playerPos = new Vector3(playerPos.x, playerPos.y,0);
        if (playerStart == transform.position)
        {
            playerStart = playerPos - transform.position;
        }

        playerPos = playerPos - transform.position;
        if (activated){
            checkAngle();
        }else{
            Destroy(gameObject, destroyTime);
        }
       

    }
}
