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
            MinigameManager.instance.LoadSideScrollingShooter();
            //minigameManager.LoadSideScrollingShooter();
        }
    }
}
