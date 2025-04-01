using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;


public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;
    [SerializeField] private GameObject objMazeScene;   // The parent empty of the maze scene

    private GameObject touchedMinigame;
    private GameObject lastMinigame;
    
    private PlayerMovement mazePlayer;

    [SerializeField]
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
            SetNodeColor(lastMinigame, true);
        }
        lastMinigame = minigame;
        
        SetNodeColor(minigame, false);
        
        // pass object to mediator
        MediatorScript.instance.GetMinigame(minigame);
    }

    /// <summary>
    /// Disables or enables a node
    /// </summary>
    /// <param name="node"></param>
    /// <param name="state"></param>
    private void SetNodeColor(GameObject node, bool state)
    {
        node.GetComponent<Collider2D>().enabled = state;
        node.GetComponent<SpriteRenderer>().color = state ? minigameColor : Color.grey;
        node.GetComponentInChildren<Light2D>().enabled = state;
    }

    // Load shooter minigame
    public void LoadMinigame()
    {
        MediatorScript.instance.MazeToBulletHell();
    }
}
