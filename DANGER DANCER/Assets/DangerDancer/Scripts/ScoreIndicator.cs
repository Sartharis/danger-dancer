using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreIndicator : MonoBehaviour {

    [SerializeField] float MaxZScale;
    [SerializeField] Color MaxColor;
    [SerializeField] Color MinColor;

    private MeshRenderer mesh;
    private float currentRatio = 0;
    private float targetRatio = 0;

	// Use this for initialization
	void Start ()
    {
		mesh = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {

        targetRatio = ScoreManager.Instance.GetScoreRatio();
        currentRatio = Mathf.Lerp(currentRatio, targetRatio, 0.1f);

		mesh.material.color = MinColor + (MaxColor - MinColor) * currentRatio;
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, MaxZScale * currentRatio);
	}
}
