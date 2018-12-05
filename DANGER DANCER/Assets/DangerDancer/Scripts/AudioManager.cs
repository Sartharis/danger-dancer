using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : UnitySingleton<AudioManager>
{

    public AudioClip poseSound;
    public AudioClip ringSound;

    private AudioSource source;
    
    public void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.5f;
    }

    public void PlayAudio(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
