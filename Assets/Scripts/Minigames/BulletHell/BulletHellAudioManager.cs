using UnityEngine;

public class BulletHellAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip minigameClip;
    [SerializeField] private AudioClip minigameClip2;
    [SerializeField] private AudioSource sourceBGM;

    public void StartMusic()
    {
        sourceBGM.volume = 0.05f;

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
