using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWithDelay : MonoBehaviour {
  public float delay = 5;
  public int sceneIndex;
	// Use this for initialization
	void Start () {
		StartCoroutine(LoadLevelAfterDelay(delay, sceneIndex));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
  IEnumerator LoadLevelAfterDelay(float delay, int sceneIndex)
     {
         yield return new WaitForSeconds(delay);
         SceneManager.LoadScene(sceneIndex);
     }
}