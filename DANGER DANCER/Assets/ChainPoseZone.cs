using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChainPoseZone : MonoBehaviour
{
    [SerializeField] private ChainPoseZone chainpose;
    bool pose = false;
    private float z;
    public int chainLeftToCreate = 4;
    readonly float[] nextInChain = { 0, 90, 180, 270};
    private SpriteEffects effects;
    bool isCreated = false;

    private void Start()
    {
        effects = GetComponent<SpriteEffects>();
        z = transform.localRotation.z;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerDancer player = collision.GetComponent<PlayerDancer>();
        if (player && player.actionState == EActionState.AS_POSE && !pose)
        {
            OnPose();
        }
    }

    public virtual void OnPose()
    {
        ScoreManager.Instance.AddScore(15, "Pose Zone", transform.position);
        pose = true;
    }

    public void Update()
    {
        if (pose == true)
        {
            if (chainLeftToCreate != 0 && !isCreated)
            {
                int mask = LayerMask.GetMask("Wall");
                int randomint = Random.Range(0, 3);
                ChainPoseZone clone;
                Vector3 offset = transform.rotation * (new Vector3(2.5f, 0, 0));
                while (Physics2D.Linecast(transform.position,transform.position+offset,mask))
                {
                    randomint = Random.Range(0, 3);
                    offset = transform.rotation * (new Vector3(2.5f, 0, 0));
                }
                clone = Instantiate(chainpose, transform.position + offset, transform.rotation * Quaternion.Euler(0, 0, nextInChain[randomint]));
                clone.chainLeftToCreate--;
                isCreated = true;
            }
            Destroy(gameObject);

        }
    }
}
