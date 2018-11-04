using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseZoneTutorial : PoseZone
{

    public override void OnPose()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        TutorialManager.Instance.NextPhase();
        base.OnPose();
    }
}