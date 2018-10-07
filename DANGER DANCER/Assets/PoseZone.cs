using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseZone : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 1000f;
    [SerializeField] private float destroyTime = 0.5f;
    [SerializeField] private float acceleration = -2000f;
    bool pose = false;
    private SpriteEffects effects;

    private void Start()
    {
        effects = GetComponent<SpriteEffects>();
    }


    public void OnPose()
    {
        ScoreManager.Instance.AddScore(20, "Pose Zone", transform.position);
        pose = true;
        effects.rippleDeformX = -0.1f;
        effects.rippleDeformY = -0.1f;
    }

    public void Update()
    {
        if (pose == true){
            transform.Rotate(new Vector3(0f, 0f, spinSpeed * Time.deltaTime));
            spinSpeed += acceleration * Time.deltaTime;
            Destroy(gameObject, destroyTime);
        }
    }
}
