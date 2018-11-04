using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveZone : MonoBehaviour {

    [SerializeField]
    private float destroyTime = 0.5f;
    private bool spin = false;
    private SpriteEffects effects;

    private void Start()
    {
        effects = GetComponent<SpriteEffects>();
    }


    void OnTriggerStay2D(Collider2D other)
    {

        PlayerDancer dancer = other.GetComponent<PlayerDancer>();
        if (dancer)
        {
            if (dancer.actionState == EActionState.AS_IDLE || dancer.actionState == EActionState.AS_POSE)
            {
                dancer.Fall(new Vector2(transform.position.x - dancer.transform.position.x, transform.position.y - dancer.transform.position.y));
                Destroy(gameObject, destroyTime);
            }
        }

    }
    public void Update()
    {

    }
}
