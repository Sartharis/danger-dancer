﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ETutorialPhase
{
    TP_START,
    TP_WALK,
    TP_POSE,
    TP_SPIN,
    TP_END
}

public class TutorialManager : UnitySingleton<TutorialManager>
{
    [SerializeField] private Transform moveTutorialPrefab;
    [SerializeField] private Transform poseZoneTutorialPrefab;
    [SerializeField] private Transform spinRingTutorialPrefab;
    [SerializeField] private float poseZoneTutorialDelay;
    [SerializeField] private float spinRingTutorialDelay;
    [SerializeField] private float endTutorialDelay;
    [SerializeField] private Transform poseZoneSpawnPoint;
    [SerializeField] private Transform spinRingSpawnPoint;
    private ETutorialPhase phase = ETutorialPhase.TP_START;

    public void Start()
    {
        if(!GameManager.Instance.didTutorial)
        {
            NextPhase();
            ScoreManager.Instance.reduceScore = false;
        }
        else
        {
            SpawnManager.Instance.StartSpawning();
            TileManager.Instance.StartSpawning();
            ScoreManager.Instance.reduceScore = true;
        }
    }

    public void NextPhase()
    {
        StartCoroutine(DoNextPhase());
    }

    IEnumerator DoNextPhase()
    {
        switch (phase)
        {
            case ETutorialPhase.TP_START:
                {
                    phase = ETutorialPhase.TP_WALK;
                    Instantiate(moveTutorialPrefab, poseZoneSpawnPoint.transform.position, poseZoneSpawnPoint.rotation);
                    break;
                }
            case ETutorialPhase.TP_WALK:
                {
                    yield return new WaitForSeconds(poseZoneTutorialDelay);
                    Instantiate(poseZoneTutorialPrefab, poseZoneSpawnPoint.transform.position, poseZoneSpawnPoint.rotation);
                    phase = ETutorialPhase.TP_POSE;
                    break;
                }
            case ETutorialPhase.TP_POSE:
                {
                    yield return new WaitForSeconds(spinRingTutorialDelay);
                    Instantiate(spinRingTutorialPrefab, spinRingSpawnPoint.transform.position, poseZoneSpawnPoint.rotation);
                    phase = ETutorialPhase.TP_SPIN;
                    break;
                }
            case ETutorialPhase.TP_SPIN:
                {
                    yield return new WaitForSeconds(endTutorialDelay);
                    GameManager.Instance.didTutorial = true;
                    SpawnManager.Instance.StartSpawning();
                    TileManager.Instance.StartSpawning();
                    ScoreManager.Instance.reduceScore = true;
                    phase = ETutorialPhase.TP_END;
                    break;
                }
        }
        yield return 0;
    }

}
