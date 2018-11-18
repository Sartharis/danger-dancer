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
    readonly float[] nextInChain = {0, 90, 180, 270};
    private SpriteEffects effects;
    bool isCreated = false;

    private void Start()
    {
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
            ChainPoseZone clone;
            PoseZone otherclone;
            Vector3 offset = transform.rotation * (new Vector3(2.5f, 0, 0));
            if (chainLeftToCreate == 1)
            {
                otherclone = Instantiate(posezone, transform.position + offset, transform.rotation);
            }
            else
            {
                clone = Instantiate(chainpose, transform.position + offset, transform.rotation);
                clone.chainLeftToCreate--;
            }
            isCreated = true;
            Destroy(gameObject);
        }
       
     }

}
