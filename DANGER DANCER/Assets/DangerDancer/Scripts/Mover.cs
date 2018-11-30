using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct moverNode{
    public Vector3 position;
    public float zRotation;
    public bool Clockwise;
    public moverNode(Vector3 p,float zrot, bool isClockwise)
    {
        zRotation = zrot;
        Clockwise = isClockwise;
        position = p;
    }
}

public class Mover : MonoBehaviour
{
    [SerializeField] List<moverNode> Path;
    [SerializeField] float speed;
    float timeElapsed;
    int numberOfNodes;
    [SerializeField] int currentNodeIndex;

    moverNode currentNode;
    moverNode nextNode;
    float timeUntilNextNode;
    float angleDifference;
    Quaternion targetRot;

    void Start()
    {
        if(System.Math.Abs(speed) < .01)
        {
            speed = 10;
        }
        timeElapsed = 0;

        numberOfNodes = Path.Count;

        if(numberOfNodes == 0){
            Path.Add(new moverNode(transform.position,0,true));
            numberOfNodes = Path.Count;
        }
        currentNode = Path[currentNodeIndex];
        nextNode = Path[(currentNodeIndex+1) % numberOfNodes];
        timeUntilNextNode = (Vector3.Distance(currentNode.position, nextNode.position)/speed);
        transform.rotation = Quaternion.Euler(0, 0, currentNode.zRotation);
        targetRot = transform.rotation;

    }
    float getAngleDifference(float cRot, float nRot, bool clockwise)
    {
        float angDiff = nRot - cRot;
        //if (clockwise)
        //{
        //    if (angDiff < 0)
        //    {
        //        angDiff = 360 - angDiff;
        //    }
        //}
        //else
        //{
        //    if (angDiff > 0)
        //    {
        //        angDiff = angDiff - 360;
        //    }
        //}
        return angDiff;
    }
    // Update is called once per frame
    void cycleNodes(){
        currentNodeIndex = (1+ currentNodeIndex) % numberOfNodes;
        currentNode = nextNode;
        nextNode = Path[(1+currentNodeIndex) % numberOfNodes];
        timeUntilNextNode = (Vector3.Distance(currentNode.position, nextNode.position) / speed);
        targetRot = Quaternion.Euler(0, 0, currentNode.zRotation);
        //angleDifference = getAngleDifference(currentNode.zRotation, nextNode.zRotation,currentNode.Clockwise);
    }
    float getRotationLinear(float cRot)
    {
        return cRot+(((timeUntilNextNode - timeElapsed) / timeUntilNextNode)*angleDifference);
    }
    Vector3 getPositionLinear(Vector3 cNode, Vector3 nNode)
    {
        return (cNode * ((timeUntilNextNode - timeElapsed) / timeUntilNextNode)) + nNode * (timeElapsed / timeUntilNextNode);
    }
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed < timeUntilNextNode)
        {
            targetRot = Quaternion.Euler(0, 0, getRotationLinear(currentNode.zRotation));
            transform.position = getPositionLinear(currentNode.position,nextNode.position);
        }
        else{
            cycleNodes();
            timeElapsed = 0;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.1f);
    }
}
