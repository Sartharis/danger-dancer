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
    public bool spin = false;
    private SpriteEffects effects;

    public float timeLeft = 8;

    private void Start()
    {
        effects = GetComponent<SpriteEffects>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerDancer player = collision.GetComponent<PlayerDancer>();
        if (player && player.actionState == EActionState.AS_SPIN && Mathf.Abs(Vector2.Dot(player.moveDir, transform.right)) > 0.1f)
        {
            OnSpin();
        }
    }

    public void OnSpin()
    {
        if(!spin)
        {
            CameraShake.Instance.ShakeCamera(0.2f,0.05f);
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
        else
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                //DestEvent(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
