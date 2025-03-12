// PlayerController.cs
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public int maxLives = 3;
    private int currentLives;
    
    [Header("Gameplay Area")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("References")]
    public GameObject hitEffect;
    public AudioClip hitSound;
    
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private GameManager gameManager;
    
     // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        gameManager = FindObjectOfType<GameManager>();
        currentLives = maxLives;
        
        // Update UI
        if (gameManager != null)
        {
            gameManager.UpdateLivesUI(currentLives);
        }
    }
    
    void Update()
    {
        // Player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        
        // Normalize diagonal movement
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }
        
        rb.linearVelocity = movement * moveSpeed;
        
        // Clamp position to gameplay area
        Vector2 position = rb.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        rb.position = position;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(other.gameObject);
        }
    }
    
    public void TakeDamage()
    {
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