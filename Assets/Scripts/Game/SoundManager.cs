using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] playerSteps = new AudioClip[6];
    public AudioClip playerStep;
    [SerializeField] private AudioSource source;

    public void PlayStep()
    {
        int index = Random.Range(0, 6);

        source.clip = playerSteps[index];
        source.Play();
    }
}
