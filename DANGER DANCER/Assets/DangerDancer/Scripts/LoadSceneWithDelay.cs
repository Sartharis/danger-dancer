using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWithDelay : MonoBehaviour {
  public float delay = 1;
  private bool canSkip = false;
  public int sceneIndex;
	// Use this for initialization
	void Start () {
		StartCoroutine(LoadLevelAfterDelay(delay, sceneIndex));
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.anyKey && canSkip)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

  IEnumerator LoadLevelAfterDelay(float delay, int sceneIndex)
     {
         yield return new WaitForSeconds(delay);
         canSkip = true;
     }
}