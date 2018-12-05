using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : UnitySingleton<LevelManager>
{
    public int levelIndex;
    public Song levelSong;
    public Song winSong;
    public Song loseSong;
    public Song tutorialSong;
    public dialogue startDialogue;
    public bool roundLost = false;
    public bool roundWin = false;
    private bool restarted = false;
    public bool levelStarted = false;
    private bool startedMusic = false;
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
        bool seenDialogue = false;
        if(!GameManager.Instance.skipDialogue && (!GameManager.Instance.sawDialogue.TryGetValue(levelIndex, out seenDialogue) || !seenDialogue))
        {
            StartDialogue();
            GameManager.Instance.sawDialogue.Add(levelIndex, true);
        }
        else if (!GameManager.Instance.didTutorial)
        {
            StartTutorial();
        }
        else
        {
            StartLevel();
        }
    }

    public void FinishDialogue()
    {
        if (!GameManager.Instance.didTutorial)
        {
            StartTutorial();
        }
        else
        {
            StartLevel();
        }
    }

    void StartDialogue()
    {
        if(startDialogue)
        {
            BeatManager.Instance.PlaySong(tutorialSong);
            ScoreManager.Instance.reduceScore = false;
            startedMusic = true;
            startDialogue.StartDialogue();
        }
        else if (!GameManager.Instance.didTutorial)
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
        if(!startedMusic)
        {
            startedMusic = true;
            BeatManager.Instance.PlaySong(tutorialSong);
        }
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
            if(levelRestartTime < 0 && Input.anyKey)
            {
                restarted = true;
                roundLost = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        
	}

    public void LoseRound()
    {
        if(!roundLost)
        {
            BeatManager.Instance.PlaySong(loseSong, 0);
            roundLost = true;
            SpawnManager.Instance.StopSpawning();
        }
    }

    public void Win()
    {
        if (!roundWin)
        {
            StartCoroutine(WinRound());
            roundWin = true;
        }
    }

    IEnumerator WinRound()
    {
        ScoreManager.Instance.reduceScore = false;
        BeatManager.Instance.PlaySong(winSong, 0);
        yield return new WaitForSeconds(6.5f);
        SceneManager.LoadScene("LevelComplete");
    }
}
