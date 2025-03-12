// PlayerController.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public int maxLives = 3;
    public int currentLives;
    
    [Header("Gameplay Area")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Input")] 
    private float horizontalInput;
    private float verticalInput;
    private Vector2 movement;

    [Header("References")]
    public GameObject hitEffect;
    public AudioClip hitSound;
    
    [SerializeField] private Rigidbody2D rbPlayer;
    private AudioSource audioSource;
    
    [SerializeField] private GameObject objGameManager;
    private GameManager gameManager;

    void Awake()
    {
        
    }
    
    void Start()
    {
        // Set references
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        gameManager = objGameManager.GetComponent<GameManager>();
        
        currentLives = maxLives;
        
        // Update UI
        gameManager.UpdateLivesUI(currentLives);
    }

    // Movement using input system instead of hardcoded buttons
    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        
        // Get X and Y axis of input
        horizontalInput = input.x;
        verticalInput = input.y;
        
        movement = new Vector2(horizontalInput, verticalInput);
        
        // Normalize diagonal movement
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        
        rbPlayer.linearVelocity = movement * moveSpeed;
        
        // Clamp position to gameplay area
        Vector2 position = rbPlayer.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        rbPlayer.position = position;
    }
    
    // Take damage when the player touches a bullet and delete the bullet
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }
    
    private void TakeDamage()
    {
        Debug.Log("Hit!");
        currentLives--;
        
        // Play hit effect
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
        
        // Play hit sound
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        
        // Update UI
        if (gameManager != null)
        {
            gameManager.UpdateLivesUI(currentLives);
        }
        
        // Check for game over
        if (currentLives <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Trigger game over
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        
        // Optional: Disable player controls or trigger death animation
        this.enabled = false;
        
        // Optional: Destroy player with delay or deactivate
        // Destroy(gameObject, 1f);
        gameObject.SetActive(false);
    }
}