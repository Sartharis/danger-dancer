using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapSpeedModifier : MonoBehaviour
{
    [SerializeField] float speedModifyAmount;
    [SerializeField] float accelerationModifier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if(player)
            {
                player.ModifySpeed(speedModifyAmount);
                player.ModifyAcceleration(accelerationModifier);
                player.bodyshaker.shake += 0.1f;
            }
            else
            {
                Debug.LogWarning("Entity has player tag but no player component.");
            }
        }
        else
        {
            PlayerChaser chaser = collision.transform.GetComponent<PlayerChaser>();
            if(chaser)
            {
                chaser.ModifySpeed(speedModifyAmount * 1.4f);
            }
        }
    }
}
