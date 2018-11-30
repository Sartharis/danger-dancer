 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRound : MonoBehaviour {
    Vector3 playerStart;
    [SerializeField] private float destroyTime = 0.5f;
    Vector3 playerPos;
    public Sprite flipped;
    public Sprite unflipped;
    float angle;
    bool fullCircle = false;
    float prevAngle;
    public bool activated=true;
    SpriteRenderer sprite;
    float startAngle; 

    // Use this for initialization
    void Start () {
        prevAngle = 0;
        startAngle = Vector2.SignedAngle(new Vector2(playerPos.x, playerPos.y), new Vector2(playerStart.x, playerStart.y) );
        playerStart = transform.position;

        Transform player = FindObjectOfType<PlayerDancer>().transform;
        Vector3 toPlayer = transform.position - player.position;
        Vector3 rotate = Vector3.RotateTowards(transform.forward, toPlayer, 1, 0);
        Quaternion rotation = Quaternion.LookRotation(rotate, Vector3.forward);
        rotation.x = 0;
        rotation.y = 0;
        transform.rotation = rotation;

        sprite = GetComponent<SpriteRenderer>();
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

            

            if( fullCircle )
            {
                sprite.sprite = angle < 0 ? flipped : unflipped;
                sprite.material.SetFloat ("_Cutoff", ((Mathf.Abs(angle))/ 360f));
            }
            else
            {
                sprite.sprite = angle < 0 ? unflipped : flipped;
                sprite.material.SetFloat ("_Cutoff", 1 - (Mathf.Abs(angle)/ 360f));
            }
            
        }else{
            Destroy(gameObject, destroyTime);
        }


    }
}
