using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Song : System.Object
{
    [SerializeField] public float BPM;
    [SerializeField] public AudioClip audioPlayed;
    [SerializeField] public bool loop;
    [SerializeField] public float duration;
};

public class BeatManager : UnitySingleton<BeatManager>
{
    public float beatMissGraceTime;

    public delegate void Beat();
    public event Beat OnBeat;

    private Song currentSong;
    private float currentTimePerBeat;
    private float beatTimer;
    private AudioSource songPlayer;
    private float timeLeft;
    private bool playingSong;
    private bool loop;

    void Start()
    {
        beatTimer = 0;
        playingSong = false;
        songPlayer = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
    }

    public void PlaySong(Song song)
    {
        currentSong = song;
        beatTimer = 0;
        currentTimePerBeat = 1 / (song.BPM / 60.0f); 
        songPlayer.Stop();
        songPlayer.clip = song.audioPlayed;
        songPlayer.Play(0);
        playingSong = true;
        timeLeft = song.duration;
        loop = song.loop;
    }

    void Update ()
    {
        if(playingSong)
        {
            beatTimer += Time.deltaTime;
            if (beatTimer >= currentTimePerBeat)
            {
                beatTimer = beatTimer - currentTimePerBeat;
                OnBeat();
            }
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                if(loop)
                {
                    PlaySong(currentSong);
                }
            }
        }
    }
    public float getBeat(){
        return beatTimer;
    }

    public float getBPM(){
        return currentSong.BPM;
    }
    public bool IsOnBeat()
    {
        return beatTimer <= beatMissGraceTime || beatTimer >= (currentTimePerBeat - beatMissGraceTime);
    }
}
