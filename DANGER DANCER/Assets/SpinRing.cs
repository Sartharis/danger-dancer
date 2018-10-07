using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinRing : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed = 1000f;
    [SerializeField]
    private float destroyTime = 0.5f;
    [SerializeField]
    private float acceleration = -2000f;
    private bool spin = false;
    private SpriteEffects effects;

    private void Start()
    {
        effects = GetComponent<SpriteEffects>();
    }

    public void OnSpin()
    {
        if(!spin)
        {
            ScoreManager.Instance.AddScore(10, "Spin Ring", transform.position);
            spin = true;
        }

    }

    public void Update()
    {
        if (spin == true)
        {
            transform.Rotate(new Vector3(0f, 0f, spinSpeed * Time.deltaTime));
            spinSpeed += acceleration * Time.deltaTime;
            transform.localScale *= 0.95f;
            effects.enabled = false;
            Destroy(gameObject, destroyTime);
        }
    }
}
