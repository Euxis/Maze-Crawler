using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] playerSteps = new AudioClip[6];
    public AudioClip playerStep;
    public AudioClip bgmHacking;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    public void PlayBGM()
    {
        Debug.Log("Playing BGM");
        bgmSource.clip = bgmHacking;
        bgmSource.loop = true;
        bgmSource.PlayOneShot(bgmSource.clip);
    }

    public void PlayStep()
    {
        int index = Random.Range(0, 6);
        
        sfxSource.clip = playerSteps[index];
        sfxSource.volume = 0.5f;
        sfxSource.PlayOneShot(sfxSource.clip);
    }
}
