using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : UnitySingleton<ScoreManager>
{
    public int score = 0;

	
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void AddScore(int addScore, string reason, Vector3 position)
    {
        score += addScore;
        GameObject instance = Instantiate(Resources.Load("FloatingText", typeof(GameObject))) as GameObject;
        instance.transform.position = position;
        TextMesh text = instance.GetComponent<TextMesh>();
        text.text = reason + ": " + addScore.ToString();
    }
}
