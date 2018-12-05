using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChainPoseZone : MonoBehaviour
{
    [SerializeField] private ChainPoseZone chainpose;
    [SerializeField] private PoseZone posezone;
    bool pose = false;
    private float z;
    public int chainLeftToCreate = 4;
    protected readonly float[] nextInChain = {0, 90, 180, 270};
    protected SpriteEffects effects;
    bool isCreated = false;
    protected Vector3 targetPoint;

    private void Start()
    {
        targetPoint = transform.position;
        effects = GetComponent<SpriteEffects>();
        int mask = LayerMask.GetMask("Wall");
        int randomint = Random.Range(0, 3);
        transform.rotation = Quaternion.Euler(0, 0, nextInChain[randomint]);
        Vector3 offset = transform.rotation * (new Vector3(3.5f, 0, 0));
        while (Physics2D.Linecast(transform.position, transform.position + offset, mask))
        {
            randomint = Random.Range(0, 3);
            transform.rotation = Quaternion.Euler(0, 0, nextInChain[randomint]);
            offset = transform.rotation * (new Vector3(3.5f, 0, 0));
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerDancer player = collision.GetComponent<PlayerDancer>();
        if (player && (player.actionState == EActionState.AS_POSE || player.actionState == EActionState.AS_IDLE) && !pose)
        {
            OnPose();
        }
    }

    public virtual void OnPose()
    {
        if( (targetPoint - transform.position).magnitude < 0.1f)
        {
            ChainPoseZone clone;
            Vector3 offset = transform.rotation * (new Vector3(2.5f, 0, 0));
            
            if (chainLeftToCreate == 1)
            {
                ScoreManager.Instance.AddScore(15, "Chained Pose Zone", transform.position);
                Destroy(gameObject);
            }
            else
            {
                ScoreManager.Instance.AddScore(10, "Chained Pose Zone", transform.position);
                targetPoint = transform.position + offset;
                int mask = LayerMask.GetMask("Wall");
                int randomint = Random.Range(0, 3);
                transform.rotation = Quaternion.Euler(0, 0, nextInChain[randomint]);
                Vector3 offset2 = transform.rotation * (new Vector3(3.5f, 0, 0));
                while (Physics2D.Linecast(transform.position + offset, transform.position + offset + offset2, mask))
                {
                    randomint = Random.Range(0, 3);
                    transform.rotation = Quaternion.Euler(0, 0, nextInChain[randomint]);
                    offset2 = transform.rotation * (new Vector3(3.5f, 0, 0));
                }
                
            }
            chainLeftToCreate--;
            effects.deformX += 0.35f;
            effects.deformY += 0.35f;
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, 0.1f);
    }

}
