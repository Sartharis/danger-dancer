﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : UnitySingleton<ScoreManager>
{
    public float audienceScore = 50;
    public float audienceScoreMax = 100;
    public float audienceBoredomRate = 10f;
    public bool reduceScore = true;

    void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(reduceScore)
        {
            audienceScore -= audienceBoredomRate * Time.deltaTime;
            if (audienceScore <= 0.0f)
            {
                LevelManager.Instance.LoseRound();
            }
        }
    }

    public void AddScore(int addScore, string reason, Vector3 position)
    {
        audienceScore += addScore;
        if( audienceScore > audienceScoreMax)
        {
            audienceScore = audienceScoreMax;
        }
        GameObject instance = Instantiate(Resources.Load("FloatingText", typeof(GameObject))) as GameObject;
        instance.transform.position = position;
        TextMesh text = instance.GetComponent<TextMesh>();
        text.text = reason + ": " + addScore.ToString();
    }
}
