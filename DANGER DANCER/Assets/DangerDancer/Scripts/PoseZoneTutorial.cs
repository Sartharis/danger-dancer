using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseZoneTutorial : PoseZone
{

    public override void OnPose()
    {
        Destroy(transform.GetChild(0).gameObject);
        Destroy(transform.GetChild(1).gameObject);
        TutorialManager.Instance.NextPhase();
        base.OnPose();
    }


}