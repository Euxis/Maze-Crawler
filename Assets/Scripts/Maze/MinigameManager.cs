using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;
    [SerializeField] private GameObject objMazeScene;   // The parent empty of the maze scene

    private GameObject touchedMinigame;
    private GameObject lastMinigame;
    
    private PlayerMovement mazePlayer;

    private Color minigameColor;
    
    [SerializeField]
    private SoundManager soundManager;

    
    // Create a singleton minigame manager
    private void Awake()
    {
        if (instance == null) instance = this;

    }

    private void Start()
    {
        //soundManager.PlayBGM();

    }

    public void Quit(InputAction.CallbackContext context)
    {
        if(context.performed) Application.Quit();
    }

    /// <summary>
    /// Passes the touched minigame to the mediator for destruction upon minigame completion
    /// </summary>
    /// <param name="minigame"></param>
    public void MinigameTouched(GameObject minigame)
    {
        // if the current minigame touched is different from the previous minigame, re-enable the collider
        if (lastMinigame && lastMinigame != minigame)
        {
            lastMinigame.GetComponent<Collider2D>().enabled = true;
        }
        lastMinigame = minigame;

        // disable collider of minigame if the player fails, so they don't reenter the game
        minigame.GetComponent<Collider2D>().enabled = false;
        //minigame.GetComponent<SpriteRenderer>().color = Color.white;
        
        // pass object to mediator
        MediatorScript.instance.GetMinigame(minigame);
        
        
    }
    
    // Load shooter minigame
    public void LoadMinigame()
    {
        MediatorScript.instance.MazeToBulletHell();
    }
}
