using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum laserState
{
    OFF,
    ON
}

public class Laser : MonoBehaviour {
	BoxCollider2D laserCollider;
    LineRenderer laserRenderer;
    Vector2 laserOrigin;
    Vector2 laserDirection;

    [SerializeField] int timeOn;
    [SerializeField] int timeOff;
    [SerializeField] int beatSpeed=1;
    float[] cycle;
    [SerializeField] float cycleTime;
    [SerializeField]  int state;

    void Start () {
        laserCollider = GetComponent<BoxCollider2D>();
        laserRenderer = GetComponent<LineRenderer>();
        laserOrigin =transform.position;
        laserDirection=transform.up;
        cycleTime = 0;
        state = 0;
        if (beatSpeed == 0)
        {
            beatSpeed = 1;
        }
        cycle =new float[2]{timeOn, timeOff};
        BeatManager.Instance.OnBeat += laserCycleUpdate;

    }


    public void laserOn(){
        laserRenderer.enabled = true;
    }



    public void laserOff()
    {
        laserRenderer.enabled = false;
    }
    public void setLaserDirection(Vector2 newDirection){
        laserDirection = newDirection;

    }
    void laserCycleUpdate(){
        cycleTime += beatSpeed;
        if (cycleTime >= cycle[state])
        {
            cycleTime = 0;
            state = ((state + 1) % 2);
        }
    }
    void LaserUpdate(){
        if (state>=1)
        {
            laserOff();
        }else{
            laserOn();
            laserCollide();
        }
    }
    void laserCollide(){
        RaycastHit2D laserHit = Physics2D.Raycast(laserOrigin, laserDirection,1000, 1 << LayerMask.NameToLayer("Wall"));
        RaycastHit2D[] laserHits = Physics2D.LinecastAll(laserOrigin, laserHit.point);
        laserRenderer.SetPosition(0, laserOrigin);
        laserRenderer.SetPosition(1, laserHit.point);
        foreach (RaycastHit2D hit in laserHits)
        {
            Collider2D hitCollide = hit.collider;
            PlayerDancer player = hitCollide.GetComponent<PlayerDancer>();
            if(player!=null){
                player.Fall(new Vector2());
            }
                      
        }
    }
    void Update () {
        laserOrigin = transform.position;
        laserDirection = transform.up;
        LaserUpdate();
    }
}
