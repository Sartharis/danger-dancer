using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseZone : MonoBehaviour
{

	public void OnPose()
    {
        ScoreManager.Instance.AddScore(20, "Pose Zone", transform.position);
        Destroy(gameObject);
    }
}
