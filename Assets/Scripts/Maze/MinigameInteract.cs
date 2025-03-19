using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameInteract : MonoBehaviour
{
    // Load minigame when the player touches a minigame triangle
    [SerializeField] private MinigameManager minigameManager;

    public void OnTriggerEnter2D(Collider2D other)
    {
        // don't load a minigame if the minigame scene doesn't exist
        if (!SceneManager.GetSceneByName("BulletHell").IsValid()) return;
        Debug.Log("Loading minigame");
        if (other.gameObject.CompareTag("Minigame"))
        {
            // Send the gameobject to the MinigameManager for destruction if minigame is successful
            MinigameManager.instance.MinigameTouched(other.gameObject);
            
            MinigameManager.instance.LoadMinigame();
            //minigameManager.LoadSideScrollingShooter();
        }
    }
}
