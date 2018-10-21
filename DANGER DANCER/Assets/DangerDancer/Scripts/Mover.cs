using UnityEngine;
using System.Collections.Generic;

public class Mover : MonoBehaviour
{
    [SerializeField] List<Vector3> Path=new List<Vector3>();
    [SerializeField] float speed;
    float timeElapsed;
    int numberOfNodes;
    [SerializeField] int currentNodeIndex;

    Vector3 currentNode;
    Vector3 nextNode;
    float timeUntilNextNode;

    void Start()
    {
        if(System.Math.Abs(speed) < .01)
        {
            speed = 10;
        }
        timeElapsed = 0;

        numberOfNodes = Path.Count;

        if(numberOfNodes == 0){
            Path.Add(transform.position);
            numberOfNodes = Path.Count;
        }
        currentNodeIndex = 0;
        currentNode = Path[0];
        nextNode = Path[(currentNodeIndex+1) % numberOfNodes];
        timeUntilNextNode = (Vector3.Distance(currentNode, nextNode)/speed);

    }

    // Update is called once per frame
    void cycleNodes(){
        currentNodeIndex = (1+ currentNodeIndex) % numberOfNodes;
        currentNode = nextNode;
        nextNode = Path[(1+currentNodeIndex) % numberOfNodes];
        timeUntilNextNode = (Vector3.Distance(currentNode, nextNode) / speed);
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
            transform.position = getPositionLinear(currentNode,nextNode);
        }
        else{
            cycleNodes();
            timeElapsed = 0;
        }
    }
}
