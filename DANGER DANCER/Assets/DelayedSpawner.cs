using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSpawner : MonoBehaviour {
    [SerializeField] private int beatsToShrink = 2;
    [SerializeField] private Sprite goodSprite;
    [SerializeField] private Sprite badSprite;
    public bool isBad;
    public GameObject objectToSpawn;
    float scalar = 0;
    private SpriteRenderer sprite;
    // Use this for initialization
    void Start()
    {
        scalar = -transform.localScale.x / beatsToShrink;
        BeatManager.Instance.OnBeat += OnBeat;
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnBeat()
    {

        transform.localScale += new Vector3(scalar, scalar, 0);

        if (transform.localScale.x <= 0)
        {
            BeatManager.Instance.OnBeat -= OnBeat;
            if (objectToSpawn){
                Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    // Use this for initialization
	
	// Update is called once per frame
	void Update () {
		sprite.sprite = isBad ? badSprite : goodSprite;
	}
}
