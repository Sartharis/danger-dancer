using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChainPoseZoneTutorial : ChainPoseZone
{
    public override void OnPose()
    {
        if ((targetPoint - transform.position).magnitude < 0.1f)
        {
            ChainPoseZone clone;
            Vector3 offset = transform.rotation * (new Vector3(2.5f, 0, 0));

            if (chainLeftToCreate == 1)
            {
                TutorialManager.Instance.NextPhase();
                ScoreManager.Instance.AddScore(10, "Chained Pose Zone", transform.position);
                Destroy(transform.parent.gameObject);
            }
            else
            {
                ScoreManager.Instance.AddScore(5, "Chained Pose Zone", transform.position);
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
}
