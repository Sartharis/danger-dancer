using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : UnitySingletonPersistent<GameManager>
{
    public bool didTutorial = false;
    public bool skipDialogue = false;
    public int CHEATStartBeat = 0;
    public bool CHEATNoReduceScore = false;
    public Dictionary<int,bool> sawDialogue;

    private void Start()
    {
        sawDialogue = new Dictionary<int, bool>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene (0);
        }
    }
}
