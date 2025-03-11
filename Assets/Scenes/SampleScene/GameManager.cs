// GameManager.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // UI References to show on-screen information
    [Header("UI References")]
    public Text livesText; // Text component to display remaining lives
    public GameObject gameOverPanel;
    
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
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }
    
    public void GameOver()
    {
        isGameOver = true;
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Optional: Stop all bullet spawners
        BulletSpawner[] spawners = FindObjectsOfType<BulletSpawner>();
        foreach (BulletSpawner spawner in spawners)
        {
            spawner.enabled = false; // Stop each Bullet Spawner
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
