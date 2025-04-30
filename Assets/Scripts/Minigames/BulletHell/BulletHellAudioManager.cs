using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletHellAudioManager : MonoBehaviour
{
    [Header("BGM Clips")]
    [SerializeField] private AudioClip minigameClip;
    [SerializeField] private AudioClip minigameClip2;
    [SerializeField] private AudioClip minigameCollectClip;

    [Header("SFX Clips")] 
    [SerializeField] private AudioClip hurtSFX;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sourceBGM;
    [SerializeField] private AudioSource sourceSFX;
    
    private void Awake()
    {
        sourceBGM.volume = MediatorScript.instance.GetVolume();
    }

    private void OnEnable()
    {
        sourceBGM.volume = MediatorScript.instance.GetVolume();

    }

    private void Start()
    {
        sourceBGM.volume = 0.5f;
    }

    public void SetVolume(int v)
    {
        sourceBGM.volume = v;
    }

    public void StartMusic(string mode)
    {
        
        
        

        if (!MediatorScript.instance.GetBGM()) return;
        if (mode == "survive")
        {
            int randomSong = Random.Range(0, 2);
            if (randomSong == 0)
            {
                sourceBGM.PlayOneShot(minigameClip);
            }
            else
            {
                sourceBGM.PlayOneShot(minigameClip2);
            }    
        }else if (mode == "collect")
        {
            sourceBGM.clip = minigameCollectClip;
            sourceBGM.loop = true;
            sourceBGM.Play();
        }
    }

    public void PlayHurt()
    {
        sourceSFX.PlayOneShot(hurtSFX);
    }



    public void StopMusic()
    {
        sourceBGM.Stop();
    }
}
