// GameManager.cs

using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // UI References to show on-screen information
    [Header("UI References")]
    public TMP_Text livesText; // Text component to display remaining lives
    public GameObject gameOverPanel;

    public UnityEvent gameOverEvent;
    
    private bool isGameOver = false;
    
    void Start()
    {
        // Initialize UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }
    
    public void UpdateLivesUI(int lives)
    {
        livesText.text = lives.ToString();
    }
    
    public void GameOver()
    {
        isGameOver = true;
        
        gameOverEvent?.Invoke();
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Optional: Stop all bullet spawners
        BulletSpawner[] spawners = FindObjectsOfType<BulletSpawner>();
        foreach (BulletSpawner spawner in spawners)
        {
            // Stop each bullet spawner
            spawner.StopSpawning();
        }
    }
    
    public bool IsGameOver()
    {
        return isGameOver;
    }
    
    public void RestartGame()
    {
        // Reload current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    
    public void ReturnToMainMenu()
    {
        // Load main menu scene
        
        SceneManager.LoadScene("MainMenu");
    }
}
