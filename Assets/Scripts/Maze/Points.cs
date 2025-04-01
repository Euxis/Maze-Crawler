using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Points : MonoBehaviour
{
    // Get reference to points UI text
    [Header("UI Elements")]
    [SerializeField] private TMP_Text points;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text gameOver;
    [SerializeField] private TMP_Text gameComplete;
    [SerializeField] private TMP_Text bitReward;
    [SerializeField] private TMP_Text bitRewardUI;
    [SerializeField] private GameObject UI_Parent;
    
    private int pointCount;
    private int HPCount;

    [Header("Player parameters")]
    [SerializeField] private GameObject objPlayer;
    [SerializeField] private PlayerMovement playerMovement;
    
    private ScoreManager scoreManager;
    private HealthManager healthManager;
    private MazeTimer mazeTimer;
    
    private bool firstGame = true;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();   
        healthManager = GetComponent<HealthManager>();
        mazeTimer = GetComponent<MazeTimer>();    
    }

    private void Start()
    {
        gameComplete.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        healthManager.SetHealth(3);
        pointCount = 0;
    }

    /// <summary>
    /// Checks the state of the game.
    /// Updates health text color and if health leq 0, initiate game over.
    /// </summary>
    private void StateCheck()
    {
        
        if(healthManager.GetHealth() == 3) MediatorScript.instance.setShaderVars.ResetChromatic();

        if (healthManager.IsHealthZero())
        {
            // Send game over event
            SceneManager.UnloadSceneAsync(3);
            gameObject.SetActive(true);

            StartCoroutine(DoGameOver());
        }

        if (healthManager.GetHealth() ==2)
        {
            MediatorScript.instance.setShaderVars.SetChromatic(0.007f);
            healthManager.SetTextColor(Color.yellow);
            objPlayer.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (healthManager.GetHealth() == 1)
        {
            MediatorScript.instance.setShaderVars.SetChromatic(0.01f);
            healthManager.SetTextColor(Color.red);
            objPlayer.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void AddPoints(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            scoreManager.AddPoints(2000);
            StateCheck();
        }
        if (firstGame)
        {
            mazeTimer.RemoveGracePeriod();
            firstGame = false;
        }
        
    }

    public void AddPoints(int p)
    {
        scoreManager.AddPoints(p);
        
        // Spawn point text
        Instantiate(bitReward, objPlayer.transform.position, Quaternion.identity, UI_Parent.transform);
        Instantiate(bitRewardUI, points.transform.position, Quaternion.identity, points.transform);
        if (firstGame)
        {
            mazeTimer.RemoveGracePeriod();
            firstGame = false;
        }
        StateCheck();
    }
    
    /// <summary>
    /// Game over screen.
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoGameOver()
    {
        foreach(var obj in PrefabMinigame.Nodes)
        {
            obj.gameObject.SetActive(false);
        }
        gameOver.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }
    
    /// <summary>
    /// Obselete, there is no win screen now.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoGameFinish()
    {
        MediatorScript.instance.setShaderVars.ResetChromatic();

        foreach(var obj in PrefabMinigame.Nodes)
        {
            Destroy(obj);
        }

        gameComplete.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
    }

    public void StartGameOver()
    {
        StartCoroutine(DoGameOver());
    }

    public void RemoveLife()
    {
        healthManager.DeductHealth(1);
        StateCheck();
        playerMovement.ReturnLastPosition();
    }

}
