using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MediatorScript : MonoBehaviour
{
    // This script handles cross scene communication
    private int wonPoints = 0;
    
    // References to all managers
    [SerializeField]
    private GameObject bulletHell_GameManager;
    private GameObject bulletHell_ControlManager;

    [SerializeField]
    private GameObject maze_GameManager;
    private GameObject maze_ControlManager;
    
    // Manager scripts
    [SerializeField]
    private Points pointsScript;
    
    // Scene game objects
    [SerializeField]
    private GameObject maze_SceneObject;
    private GameObject bulletHell_SceneObject;
    
    // Referneces to all cameras
    [SerializeField]
    private Camera bulletHell_Camera;
    private Camera maze_Camera;
    
    private AudioListener maze_AudioListener;
    private AudioListener bulletHell_AudioListener;

    private GameObject storedMinigame;

    private void Start()
    {
        maze_SceneObject = GameObject.FindWithTag("MazeScene");
        bulletHell_SceneObject = GameObject.FindWithTag("BulletScene");
         
        bulletHell_ControlManager = GameObject.FindWithTag("BulletControl");
        bulletHell_GameManager = GameObject.FindWithTag("BulletGame");
 
        maze_ControlManager = GameObject.FindWithTag("MazeControl");
        maze_GameManager = GameObject.FindWithTag("MazeGame");
         
        bulletHell_Camera = GameObject.Find("bulletHell_Camera").GetComponent<Camera>();
        maze_Camera = GameObject.Find("maze_Camera").GetComponent<Camera>();
        maze_AudioListener = maze_Camera.GetComponent<AudioListener>();
        bulletHell_AudioListener = bulletHell_Camera.gameObject.GetComponent<AudioListener>();
 
        pointsScript = maze_GameManager.GetComponent<Points>();
        
        // Disable bullet hell minigame
        DisableBulletHell();
    }

    private void DisableBulletHell()
    {
        var bulletHell_AudioListener = bulletHell_Camera.GetComponent<AudioListener>();
        bulletHell_AudioListener.enabled = false;
        bulletHell_GameManager.SetActive(false);
        bulletHell_ControlManager.SetActive(false);
        bulletHell_SceneObject.SetActive(false);
    }

    public void GetMinigame(GameObject minigame)
    {
        storedMinigame = minigame;
    }
    
    /// <summary>
    /// The win state of a game. Will destroy the touched minigame triangle
    /// </summary>
    /// <param name="p"></param>
    public void RewardPoints(int p)
    {
        wonPoints += p;
        if (!maze_GameManager) return;
        pointsScript.AddPoints(p);
        wonPoints = 0;
        Destroy(storedMinigame);
    }

    /// <summary>
    /// The fail state of a game. Will set back the player to the previous tile so they don't enter the minigame
    /// again and deduct one life.
    /// </summary>
    /// <param name="l"></param>
    public void DeductLife()
    {
        if (!maze_GameManager) return;
        pointsScript.RemoveLife();
        

    }

    // Method to load and unload given scenes
    public void MazeToBulletHell()
    {
        // Disable gameObjects in the maze scene
        maze_AudioListener.enabled = false;
        maze_GameManager.SetActive(false);
        maze_ControlManager.SetActive(false);
        maze_SceneObject.SetActive(false);
        
        // Enable objects in bullethell
        bulletHell_AudioListener.enabled = true;
        bulletHell_ControlManager.SetActive(true);
        bulletHell_GameManager.SetActive(true);
        bulletHell_SceneObject.SetActive(true);
        

        //SceneManager.LoadSceneAsync("BulletHell", LoadSceneMode.Additive);
        bulletHell_GameManager.GetComponent<GameManager>().StartGame();
    }

    public void BulletHellToMaze()
    {
        bulletHell_AudioListener.enabled = false;
        bulletHell_GameManager.SetActive(false);
        bulletHell_ControlManager.SetActive(false);
        bulletHell_SceneObject.SetActive(false);
        
        // Enable gameObjects in the maze scene
        maze_AudioListener.enabled = true;
        maze_GameManager.SetActive(true);
        maze_ControlManager.SetActive(true);
        maze_SceneObject.SetActive(true);

        //SceneManager.UnloadSceneAsync("BulletHell");
    }
}
