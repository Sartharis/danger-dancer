using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesTile : MonoBehaviour
{

	[SerializeField] int spikeDelay = 2;
	[SerializeField] int spikeReset = 5;
	[SerializeField] Sprite inactiveSprite;
	[SerializeField] Sprite warningSprite;
	[SerializeField] Sprite activeSprite;
	int beat = 0;
	bool active = false;
	bool warning = false;

	// Use this for initialization
	void Start ()
	{
		BeatManager.Instance.OnBeat += OnBeat;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			if (!active)
			{
				warning = true;
				GetComponent<SpriteRenderer> ().sprite = warningSprite;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "Player" && active)
		{
			collision.GetComponent<PlayerDancer> ().Fall (new Vector2 ());
		}
	}

	private void OnBeat ()
	{
		if (warning)
		{
			beat++;
			if (beat >= spikeReset)
			{
				active = false;
				warning = false;
				beat = 0;
				GetComponent<SpriteRenderer> ().sprite = inactiveSprite;
			}
			else if (beat >= spikeDelay)
			{
				active = true;
				GetComponent<SpriteRenderer> ().sprite = activeSprite;
			}
		}
	}
}
