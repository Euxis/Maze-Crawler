using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] playerSteps = new AudioClip[6];
    public AudioClip playerStep;
    public AudioClip bgmHacking;
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    
    
    private bool toggleBGM = true;
    public void PlayBGM()
    {
        //Debug.Log("Playing BGM");
        bgmSource.clip = bgmHacking;
        bgmSource.loop = true;
        bgmSource.volume = 0.05f;
        bgmSource.PlayOneShot(bgmSource.clip);
    }
    

    /// <summary>
    /// Toggle BGM 
    /// </summary>
    public void ToggleBGM(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (toggleBGM)
            {
                bgmSource.volume = 0;
                toggleBGM = false;
                // Also disables BGM on minigames
                MediatorScript.instance.SetBGM(false);

            }else
            {
                bgmSource.volume = 0.05f;
                toggleBGM = true;
                // Also enables BGM on minigames
                MediatorScript.instance.SetBGM(true);
            }

        }
    }
     

    public void PlayStep()
    {
        int index = Random.Range(0, 6);
        
        sfxSource.clip = playerSteps[index];
        sfxSource.volume = 0.05f;
        sfxSource.PlayOneShot(sfxSource.clip);
    }
}
