using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] playerSteps = new AudioClip[6];
    public AudioClip playerStep;
    
    [Header("Hacking Songs")]
    [SerializeField] private AudioClip bgmHacking;
    
    // includes both start [0] and looped versions [1]
    [SerializeField] private AudioClip[] bgmHacking2 = new AudioClip[2];
    
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    private bool toggleBGM = true;

    private void Awake()
    {
        PlayBGM();
        
    }

    private void Start()
    {
        // default volume
        bgmSource.volume = 0.5f;
    }

    private void OnEnable()
    {
        if(bgmSource.isPlaying) return;
        PlayBGM();
    }

    private void Update()
    {
        if (bgmSource.clip == bgmHacking2[0] && !bgmSource.isPlaying)
        {
            bgmSource.clip = bgmHacking2[1];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    private IEnumerator WaitForLoop(float time)
    {
        yield return new WaitForSeconds(time);
        bgmSource.clip = bgmHacking2[1];
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayBGM()
    {
        StopAllCoroutines();
        var rand = Random.Range(0, 2);
        if (rand == 0)
        {
            bgmSource.loop = true;
            bgmSource.clip = bgmHacking;
        }
        else
        {
            bgmSource.loop = false;
            bgmSource.clip = bgmHacking2[0];
        }

        

        bgmSource.Play();
        //if (rand == 1) StartCoroutine(WaitForLoop(bgmHacking2[0].length));
    }

    public void SetVolume(int v)
    {
        bgmSource.volume = v / 10f;
        sfxSource.volume = v / 10f;
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
                bgmSource.volume = 5f;
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
