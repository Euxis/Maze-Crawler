using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    public UnityEvent gameStart;        // Starts all game objects in the scene

    // Audio manager
    [SerializeField] private BulletHellAudioManager bulletHellAudio;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private PlayerController playerController;
    // Byte Spawner
    [SerializeField] private ByteSpawner _byteSpawner;

    [SerializeField] private GameObject prefabParent;
    [SerializeField] private EnemySpawner enemySpawner;
    
    [Header("Spawning bounds")]
    [SerializeField] private Vector2 lowerBounds;
    [SerializeField] private Vector2 upperBounds;
    [SerializeField] private GameObject playerObject;
    
    // Position of spawners
    private int positionX;
    private int positionY;

    /// <summary>
    /// Clears all the prefabs
    /// </summary>
    public void ClearGame()
    {
        completeText.gameObject.SetActive(false);
        enemyManager.DestroyAllEnemies();
        enemyManager.ClearEnemies();
    }

    public void StartGame()
    {
        MediatorScript.instance.setShaderVars.ResetChromatic();
        // Initialize UI
        gameOverPanel.SetActive(false);
        
        MakeBulletSpawners();
        enemyManager.SetEnemiesTarget(playerObject);
        _byteSpawner.MakeBytes();
        
        // Start the countdown
        StartCoroutine(DoStartCountdown());
    }

    private void Update()
    {
        Resources.UnloadUnusedAssets();   
    }

    private bool CheckValidPosition(Vector2 position)
    {
        // Check for bullet spawners occupying the space
        var result = Physics2D.OverlapCircle(position, 1.5f, 
            LayerMask.GetMask("BulletSpawner"));

        var free = !(result && result.CompareTag("BulletSpawner") && position == Vector2.zero);
        return free;
    }
    
    /// <summary>
    /// Spawns random enemies on the field
    /// </summary>
    private void MakeBulletSpawners()
    {
        // Spawn between 4-5 enemies
        var numberOfSpawners = Random.Range(5, 8);
        enemySpawner.SpawnRandomEnemies(numberOfSpawners, 
            lowerBounds, upperBounds, 
            CheckValidPosition);
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
        enemyManager.StopAllEnemies();
        
        if (b)
        {
            StartCoroutine(DoGameComplete());
        }
        else
        {
            StartCoroutine(DoGameFail());
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
        MediatorScript.instance.RewardPoints(1);
        MediatorScript.instance.BulletHellToMaze();
    }

    private IEnumerator DoGameFail()
    {
        bulletHellAudio.StopMusic();
        // Show game over panel
        gameOverPanel.SetActive(true);
        // Invoke game over event
        gameOverEvent?.Invoke();
        ClearGame();

        yield return new WaitForSeconds(2f);

        MediatorScript.instance.DeductLife();
        MediatorScript.instance.BulletHellToMaze();
    }

    /// <summary>
    /// Starts countdown at the start of the game
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoStartCountdown()
    {
        // make sure the countdown text is active
        countdownText.gameObject.SetActive(true);
        
        // set player to active so they can preposition
        playerController.CanStart(true);
        
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        countdownText.text = "Begin";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        // If you want to move StartAllEnemies() into gameStart,
        // go ahead.
        enemyManager.StartAllEnemies();
        gameStart?.Invoke();
    }
}
