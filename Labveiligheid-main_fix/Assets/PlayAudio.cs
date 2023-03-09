using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] audioClips;
    void Start()
    {
        
    }

    public void Play()
    {
source.PlayOneShot(RandomClip());
    }

    AudioClip RandomClip()
    {
        return audioClips[Random.Range(0, audioClips.Length)];
    }
}
