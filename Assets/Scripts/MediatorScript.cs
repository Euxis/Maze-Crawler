using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MediatorScript : MonoBehaviour
{
    // This script handles cross scene communication
    private int wonPoints = 0;
    
    // Singleton
    public static MediatorScript instance;
    
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
    private GameObject options_SceneObject;
    
    // Referneces to all cameras
    [SerializeField]
    private Camera bulletHell_Camera;
    private Camera maze_Camera;
    private Camera options_Camera;
    
    private AudioListener maze_AudioListener;
    private AudioListener bulletHell_AudioListener;

    private SoundManager maze_AudioManager;
    private BulletHellAudioManager bullet_AudioManager;

    private GameObject options_ControlManager;

    private SoundManager mazeSoundManager;

    private GameObject storedMinigame;

    // Events
    public UnityEvent onWin;
    public UnityEvent onLose;

    private bool isBGM = true;
    private bool isStep = true;
    private bool isShader = true;
    private int volume = 5;
    
    // Shader settings
    public SetShaderVars setShaderVars;

    public bool GetBGM()
    {
        return isBGM;
    }

    public void SetBGM(bool value)
    {
        isBGM = value;
    }

    public void SetMusicVolume(int v)
    {
        volume = v;
        maze_AudioManager.SetVolume(v);
        bullet_AudioManager.SetVolume(v);
    }

    public int GetVolume()
    {
        return volume;
    }

    public bool GetStep()
    {
        return isStep;
    }

    public void SetStep(bool value)
    {
        isStep = value;
    }

    public bool GetShader()
    {
        return isShader;
    }

    public void SetShader(bool value)
    {
        isShader = value;
        ShaderSetting shaderSettingScript = FindAnyObjectByType<ShaderSetting>();
        shaderSettingScript.ManualUpdate(value);
        
        CheckShaderState checkShaderScript = FindAnyObjectByType<CheckShaderState>();
        checkShaderScript.ManualUpdate(value);
    }

    private void Start()
    {
        // Get all the references needed
        maze_SceneObject = GameObject.FindWithTag("MazeScene");
        bulletHell_SceneObject = GameObject.FindWithTag("BulletScene");
        options_SceneObject = GameObject.FindWithTag("OptionsScene");
        
        bulletHell_ControlManager = GameObject.FindWithTag("BulletControl");
        bulletHell_GameManager = GameObject.FindWithTag("BulletGame");
 
        maze_ControlManager = GameObject.FindWithTag("MazeControl");
        maze_GameManager = GameObject.FindWithTag("MazeGame");
         
        bulletHell_Camera = GameObject.Find("bulletHell_Camera").GetComponent<Camera>();
        maze_Camera = GameObject.Find("maze_Camera").GetComponent<Camera>();
        options_Camera = GameObject.Find("options_Camera").GetComponent<Camera>();
        
        maze_AudioListener = maze_Camera.GetComponent<AudioListener>();
        bulletHell_AudioListener = bulletHell_Camera.gameObject.GetComponent<AudioListener>();
 
        pointsScript = maze_GameManager.GetComponent<Points>();
        
        setShaderVars = this.gameObject.GetComponent<SetShaderVars>();

        bullet_AudioManager = FindAnyObjectByType<BulletHellAudioManager>();
        maze_AudioManager = FindAnyObjectByType<SoundManager>();
        
        options_ControlManager = GameObject.FindWithTag("OptionsControl");
        
        
        // Make sure chromatic abberation is reset when game starts
        setShaderVars.ResetChromatic();
        
        // Disable bullet hell minigame
        DisableBulletHell();
        options_SceneObject.SetActive(false);
        options_ControlManager.SetActive(false);

        // Set singleton
        if (instance == null) instance = this;
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
        onWin?.Invoke();
        pointsScript.AddPoints(p);
        wonPoints = 0;
        Destroy(storedMinigame);
    }

    /// <summary>
    /// The fail state of a game. Will set back the player to the previous tile so they don't enter the minigame
    /// again and deduct one life.
    /// </summary>
    public void DeductLife()
    {
        if (!maze_GameManager) return;
        pointsScript.RemoveLife();
    }

    public void MazeToBulletHell(bool b, String mode)
    {
        maze_AudioListener.enabled = !b;
        maze_GameManager.SetActive(!b);
        maze_ControlManager.SetActive(!b);
        maze_SceneObject.SetActive(!b);

        bulletHell_AudioListener.enabled = b;
        bulletHell_ControlManager.SetActive(b);
        bulletHell_GameManager.SetActive(b);
        bulletHell_SceneObject.SetActive(b);

        if (b) bulletHell_GameManager.GetComponent<GameManager>().StartGame(mode);
    }
    
    public void MazeToBulletHell(bool b)
    {
        
        bulletHell_AudioListener.enabled = b;
        bulletHell_ControlManager.SetActive(b);
        bulletHell_GameManager.SetActive(b);
        bulletHell_SceneObject.SetActive(b);
        
        maze_AudioListener.enabled = !b;
        maze_GameManager.SetActive(!b);
        maze_ControlManager.SetActive(!b);
        maze_SceneObject.SetActive(!b);

        if(!b) maze_AudioManager.PlayBGM();
    }

    public void LoadOptionsMenu(bool b)
    {
        if (b)
        {
            maze_ControlManager.SetActive(!b);
            options_ControlManager.SetActive(b);
            
        }
        else
        {
            options_ControlManager.SetActive(b);
            maze_ControlManager.SetActive(!b);
            
            
        }

        maze_AudioListener.enabled = !b;
        //maze_GameManager.SetActive(!b);
        //maze_SceneObject.SetActive(!b);
        
        options_SceneObject.SetActive(b);
        
        options_Camera.gameObject.transform.position = maze_Camera.gameObject.transform.position;
    }

    public void BulletHellToMaze()
    {
        // Disable main gameObjects in bullethell
        bulletHell_AudioListener.enabled = false;
        bulletHell_GameManager.SetActive(false);
        bulletHell_ControlManager.SetActive(false);
        bulletHell_SceneObject.SetActive(false);
        
        // Enable gameObjects in the maze scene
        maze_AudioListener.enabled = true;
        maze_GameManager.SetActive(true);
        maze_ControlManager.SetActive(true);
        maze_SceneObject.SetActive(true);
    }
}
