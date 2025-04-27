using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletHellAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip minigameClip;
    [SerializeField] private AudioClip minigameClip2;
    [SerializeField] private AudioSource sourceBGM;

    private void Awake()
    {
        sourceBGM.volume = MediatorScript.instance.GetVolume();
    }

    private void OnEnable()
    {
        sourceBGM.volume = MediatorScript.instance.GetVolume();

    }

    public void SetVolume(int v)
    {
        sourceBGM.volume = v;
    }

    public void StartMusic()
    {
        sourceBGM.volume = 5f;

        if (!MediatorScript.instance.GetBGM()) return;    
        
        int randomSong = Random.Range(0, 2);
        if (randomSong == 0)
        {
            sourceBGM.PlayOneShot(minigameClip);
        }
        else
        {
            sourceBGM.PlayOneShot(minigameClip2);
        }

    }
    
    

    public void StopMusic()
    {
        sourceBGM.Stop();
    }
}
