﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : UnitySingleton<LevelManager>
{
    public bool roundLost = false;
    private bool restarted = false;
    [SerializeField] private float levelRestartTime = 2.0f;

	// Use this for initialization
	void Start ()
    {
		roundLost = false;
        restarted = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if( !roundLost && ScoreManager.Instance.audienceScore < 0.0f)
        {
            LoseRound();
        }

        if(roundLost && !restarted)
        {
            levelRestartTime -= Time.deltaTime;
            if(levelRestartTime < 0)
            {
                restarted = true;
                roundLost = false;
                 SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
	}

    public void LoseRound()
    {
        roundLost = true;
    }

    public void WinRound()
    {

    }
}