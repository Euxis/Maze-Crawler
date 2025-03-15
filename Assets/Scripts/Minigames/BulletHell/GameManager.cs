// GameManager.cs

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    // UI References to show on-screen information
    [Header("UI References")]
    public TMP_Text livesText; // Text component to display remaining lives
    public GameObject gameOverPanel;

    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text completeText; // Game complete text

    public UnityEvent gameOverEvent;    // Stops game objects and returns to maze
    public UnityEvent gamePause;        // Pauses all game objects in the scene
    public UnityEvent gameStart;        // Starts all game objects in the scene
    
    
    private BulletSpawner[] spawners;
    [SerializeField] private GameObject bulletSpawnerPrefab;
    
    [SerializeField] private List<Vector2> spawnPoints;

    [SerializeField] private BulletHellAudioManager bulletHellAudio;
    
    private MediatorScript mediatorScript;
    
    // Position of spawners
    private int positionX;
    private int positionY;

    private void Awake()
    {   
        mediatorScript = GameObject.Find("Mediator").GetComponent<MediatorScript>();
    }

    void Start()
    {
        
    }

    public void ClearGame()
    {
        completeText.gameObject.SetActive(false);
        
        foreach (BulletSpawner spawner in spawners)
        {
            Destroy(spawner.gameObject);
        }
    }

    public void StartGame()
    {
        // Initialize UI
        gameOverPanel.SetActive(false);
        
        MakeBulletSpawners();
        
        // Get list of spawners in game
        spawners = FindObjectsOfType<BulletSpawner>();
        
        // Start the countdown
        StartCoroutine(DoStartCountdown());
    }

    private void Update()
    {
        Resources.UnloadUnusedAssets();   
    }

    /// <summary>
    /// Spawns random enemies on the field
    /// </summary>
    private void MakeBulletSpawners()
    {
        // Spawn between 4-5 enemies
        int numberOfSpawners = Random.Range(3, 5);
        
        // Get the number of BulletSpawner spawn patterns
        var uniqueSpawnPatterns = Enum.GetValues(typeof(BulletSpawner.SpawnPattern)).Length;
        
        // Randomize each stat of each prefab
        for (int i = 0; i <= numberOfSpawners; i++)
        {
            RandomizeSpawnerStats();
            
            int randomPattern = Random.Range(0, uniqueSpawnPatterns + 1);

            // create the prefab and set the spawn pattern
            var prefab = Instantiate(bulletSpawnerPrefab, new Vector2(positionX, positionY), Quaternion.identity);
            var prefabScript = prefab.GetComponent<BulletSpawner>();
            prefabScript.SetSpawnPattern(randomPattern);
        }
        spawnPoints.Clear();

    }

    private void RandomizeSpawnerStats()
    {
        int maxAttempts = 100; // Prevent infinite looping
        int attempts = 0;
        Vector2 newPosition;

        do
        {
            positionX = Random.Range(-8, 9);
            positionY = Random.Range(-4, 5);
        
            positionX = Mathf.Clamp(positionX, -8, 8);
            positionY = Mathf.Clamp(positionY, -4, 4);

            newPosition = new Vector2(positionX, positionY);
            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("RandomizeSpawnerStats exceeded max attempts!");
                break;
            }

        } while (spawnPoints.Contains(new Vector2(positionX, positionY)) && spawnPoints.Count > 0);
        
        spawnPoints.Add(newPosition);
    }

    /// <summary>
    /// Sets HP UI to current amount of HP and changes its color depending on how high HP is.
    /// </summary>
    /// <param name="lives"></param>
    /// <param name="maxLives"></param>
    public void UpdateLivesUI(int lives, int maxLives)
    {
        // More than half: green
        if (lives > (maxLives / 2) + 1) livesText.color = Color.green;
        // At halfway: yellow
        else if (lives >= (maxLives / 2) + 1) livesText.color = Color.yellow;
        // Less than half: red
        else livesText.color = Color.red;
        
        // Set text
        livesText.text = lives.ToString();
    }

    /// <summary>
    /// Ends the game, passing true if the player wins, or false if the player loses
    /// </summary>
    /// <param name="b"></param>
    public void isGameSuccess(bool b)
    {
        StartSpawning(false);
        if (b) StartCoroutine(DoGameComplete());
        else StartCoroutine(DoGameFail());
    }


    /// <summary>
    /// Sets all bullet spawners as able/unable to shoot
    /// </summary>
    /// <param name="b"></param>
    private void StartSpawning(bool b)
    {
        foreach (BulletSpawner spawner in spawners)
        {
            spawner.StartSpawning(b);
        } 
    }
    
    /// <summary>
    /// Game complete sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoGameComplete()
    {
        completeText.gameObject.SetActive(true);
        gameOverEvent?.Invoke();
        yield return new WaitForSeconds(2f);
        // Just go back to maze scene for now
        
        ClearGame();
        mediatorScript.RewardPoints(1);
        mediatorScript.BulletHellToMaze();
    }

    private IEnumerator DoGameFail()
    {
        bulletHellAudio.StopMusic();
        Debug.Log("Failed");
        // Show game over panel
        gameOverPanel.SetActive(true);
        // Invoke game over event
        gameOverEvent?.Invoke();
        yield return new WaitForSeconds(2f);

        ClearGame();
        mediatorScript.DeductLife();
        mediatorScript.BulletHellToMaze();
    }

    /// <summary>
    /// Starts countdown at the start of the game
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoStartCountdown()
    {
        StartSpawning(false);
        
        // make sure the countdown text is active
        countdownText.gameObject.SetActive(true);
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "Begin";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        StartSpawning(true);

        gameStart?.Invoke();
    }
}
