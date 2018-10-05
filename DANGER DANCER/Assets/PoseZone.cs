using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseZone : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 5f;
    [SerializeField] private float destroyTime = 0.5f;
    bool pose = false;

    public void OnPose()
    {
        ScoreManager.Instance.AddScore(20, "Pose Zone", transform.position);
        pose = true;

    }

    public void Update()
    {
        if (pose == true){
            transform.Rotate(new Vector3(0f, 0f, spinSpeed * Time.deltaTime));
            Destroy(gameObject, destroyTime);
        }
    }
}
