using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float shake;
    private Vector3 centerPos;

    private void Start()
    {
        centerPos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 shakeRandom = Random.insideUnitCircle * shake;
        transform.localPosition = new Vector3(centerPos.x + shakeRandom.x, centerPos.y  + shakeRandom.y, centerPos.z);
        shake = Mathf.Lerp(shake, 0, 0.1f);
    }

}
