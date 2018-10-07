using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Song : System.Object
{
    [SerializeField] public float audioBPM;
    [SerializeField] public AudioClip audioPlayed;
};

public class BeatManager : UnitySingleton<BeatManager>
{
    public Song startSong;
    public float beatMissGraceTime;

    public delegate void Beat();
    public event Beat OnBeat;

    private Song currentSong;
    private float currentTimePerBeat;
    private float beatTimer;
    private AudioSource songPlayer;

    void Start()
    {
        beatTimer = 0;
        songPlayer = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        PlaySong(startSong);
    }

    public void PlaySong(Song song)
    {
        currentSong = song;
        beatTimer = 0;
        currentTimePerBeat = 1 / (song.audioBPM / 60.0f); 
        songPlayer.Stop();
        songPlayer.clip = song.audioPlayed;
        songPlayer.Play(0);
    }

    void Update ()
    {
		beatTimer += Time.deltaTime;
        if( beatTimer >= currentTimePerBeat )
        {
            beatTimer = beatTimer - currentTimePerBeat;
            OnBeat();
        }
	}
    public float getBeat(){
        return beatTimer;
    }

    public float getBPM(){
        return currentSong.audioBPM;
    }
    public bool IsOnBeat()
    {
        return beatTimer <= beatMissGraceTime || beatTimer >= (currentTimePerBeat - beatMissGraceTime);
    }
}
