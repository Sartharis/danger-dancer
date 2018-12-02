using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : UnitySingleton<LevelManager>
{
    public Song levelSong;
    public Song tutorialSong;
    public bool roundLost = false;
    public bool roundWin = false;
    private bool restarted = false;
    public bool levelStarted = false;
    [SerializeField] private float levelRestartTime = 2.0f;

	// Use this for initialization
	void Start ()
    {
		roundLost = false;
        restarted = false;
        StartCoroutine(BeginLevel());
	}

    IEnumerator BeginLevel()
    {
        yield return new WaitForSeconds(0.5f);
        if (!GameManager.Instance.didTutorial)
        {
            StartTutorial();
        }
        else
        {
            StartLevel();
        }
    }

    void StartTutorial()
    {
        BeatManager.Instance.PlaySong(tutorialSong);
        ScoreManager.Instance.reduceScore = false;
        TutorialManager.Instance.StartTutorial();
    }
	
    public void OnTutorialDone()
    {
         GameManager.Instance.didTutorial = true;
         StartLevel();
    }

    public void StartLevel()
    {
        BeatManager.Instance.PlaySong(levelSong, GameManager.Instance.CHEATStartBeat);
        ScoreManager.Instance.reduceScore = true;
        SpawnManager.Instance.StartSpawning();
        levelStarted = true;
    }

	// Update is called once per frame
	void Update ()
    {
	    if( !roundLost && ScoreManager.Instance.audienceScore < 0.0f)
        {
            LoseRound();
            Debug.Log("Lose");
            GhostRecorder.Instance.endRecording(BeatManager.Instance.getCurrentBeat());
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

        if(roundWin)
        {
            SceneManager.LoadScene("LevelComplete");
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
