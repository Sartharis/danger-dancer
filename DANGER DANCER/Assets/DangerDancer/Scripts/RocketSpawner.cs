using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour {

	[SerializeField] int spawnDelay = 4;
	[SerializeField] float rocketSpeed = 5.0f;
	[SerializeField] JumpingMine spawnTarget;
    [SerializeField] AudioClip fire;


    SpriteEffects effects;
	private int spawnTime = 0;
    private AudioSource source;
	// Use this for initialization
	void Start ()
	{
        effects = GetComponent<SpriteEffects>();
        source = GetComponent<AudioSource>();
        BeatManager.Instance.OnBeat += OnBeat;
		spawnTime = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Transform player = FindObjectOfType<PlayerDancer> ().transform;
        Vector3 dirvec = player.position - transform.position;
        float angle = Mathf.Rad2Deg * Mathf.Atan2(dirvec.y, dirvec.x);
        transform.rotation = Quaternion.Euler(0, 0, angle);
	}

    void OnBeat()
    {
        spawnTime++;
        if (spawnTime >= spawnDelay)
        {
            source.PlayOneShot(fire);
            JumpingMine r = Instantiate(spawnTarget, transform.Find("Muzzle").position, transform.rotation);
            r.initialDir = transform.right;
            r.moveSpeed = rocketSpeed;
            spawnTime = 0;
            effects.deformX += 0.3f;
            effects.deformY += 0.3f;
        }
    }

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }
}
