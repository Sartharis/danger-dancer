using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float shake;
    public float shakeLerp = 0.1f;
    public float shakeJerkiness = 1.0f;
    private Vector3 centerPos;
    Vector2 shakeRandom;

    private void Start()
    {
        centerPos = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shakeRandom = Vector2.Lerp(shakeRandom, Random.insideUnitCircle * shake, shakeJerkiness);
        transform.localPosition = new Vector3(centerPos.x + shakeRandom.x, centerPos.y  + shakeRandom.y, centerPos.z);
        shake = Mathf.Lerp(shake, 0, shakeLerp);
    }

}
