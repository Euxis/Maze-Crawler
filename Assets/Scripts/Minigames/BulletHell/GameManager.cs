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

    // Position of spawners
    private int positionX;
    private int positionY;
    
    void Start()
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
    /// The fail state procedure.
    /// </summary>
    public void GameOver()
    {
        // Show game over panel
        gameOverPanel.SetActive(true);
        
        // Stop all bullet spawners
        StartSpawning(false);

        // Invoke game over event
        gameOverEvent?.Invoke();
    }

    /// <summary>
    /// The success state procedure.
    /// </summary>
    public void GameComplete()
    {
        StartSpawning(false);
        StartCoroutine(DoGameComplete());
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
        gamePause?.Invoke();
        yield return new WaitForSeconds(1f);
        // Just go back to maze scene for now
        SceneManager.LoadScene(1);
    }
    
    /// <summary>
    /// Starts countdown at the start of the game
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoStartCountdown()
    {
        StartSpawning(false);
        countdownText.text = "3";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "2";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "1";
        yield return new WaitForSeconds(0.5f);
        countdownText.text = "Begin";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
        StartSpawning(true);

        gameStart?.Invoke();
    }
}
