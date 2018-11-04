using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSpawner : MonoBehaviour {
    [SerializeField] private Sprite goodSprite;
    [SerializeField] private Sprite badSprite;
    Vector3 targetScale;
    public bool isBad;
    public GameObject objectToSpawn;
    int beats;
    float scalar = 0;
    private SpriteRenderer sprite;
    Vector3 initialScale;
    // Use this for initialization
    void Start()
    {
        initialScale = transform.localScale;
       
        scalar = -transform.localScale.x / SpawnManager.Instance.numDelay;
        BeatManager.Instance.OnBeat += OnBeat;
        sprite = GetComponent<SpriteRenderer>();
        beats = SpawnManager.Instance.numDelay;
         targetScale = initialScale;
    }

    void OnBeat()
    {
        beats--;
        targetScale= beats / SpawnManager.Instance.numDelay * initialScale;
        if (beats <= 0)
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
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 0.1f);
    }
}
