using System;
using Unity.VisualScripting;
using UnityEngine;

public class MinigameInteract : MonoBehaviour
{
    // Load minigame when the player touches a minigame triangle
    [SerializeField] private MinigameManager minigameManager;

    public void OnTriggerEnter2D(Collider2D other)
    {
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
