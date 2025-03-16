// PlayerController.cs

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;

    public float hurtSpeed = 3f;    // Speed when player is in invincibility window
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
    private Vector2 playerPosition;

    [Header("References")]
    public GameObject hitEffect;
    public AudioClip hitSound;
    
    [SerializeField] private Rigidbody2D rbPlayer;
    private AudioSource audioSource;
    
    [SerializeField] private GameObject objGameManager;
    [SerializeField] private GameManager gameManager;
    
    // Sprite renderer things
    [SerializeField] private SpriteRenderer spriteRendererPlayer;
    private Color colorPlayer;
    // Transparency for when the player is invincible after being hit
    private float invincibleAlphaVal = 0.5f;
    
    // Add invincibility window when player gets hit
    [SerializeField] private bool isInvincible = false;

    private void Awake()
    {
        gameObject.SetActive(true);
    }

    void Start()
    {
        // Set references
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        
        currentLives = maxLives;

        // Update UI
        gameManager.UpdateLivesUI(currentLives, maxLives);
        
        // Get original color of player
        colorPlayer = spriteRendererPlayer.color;
        gameObject.SetActive(true);
        this.enabled = false;
    }

    public void ResetStats()
    {
        rbPlayer.gameObject.transform.position = Vector2.zero;
        currentLives = maxLives;
        gameManager.UpdateLivesUI(currentLives, maxLives);
        spriteRendererPlayer.color = colorPlayer;
        isInvincible = false;
        moveSpeed = 5f;
    }

    /// <summary>
    /// Enables player to move
    /// </summary>
    /// <param name="b"></param>
    public void CanStart(bool b)
    {
        gameObject.SetActive(b);
        currentLives = maxLives;
        this.enabled = b;
    }

    /// <summary>
    /// Top down movement using new input system.
    /// </summary>
    /// <param name="context"></param>
    public void Movement(InputAction.CallbackContext context)
    {
        
        Vector2 input = context.ReadValue<Vector2>();
        
        // Get X and Y axis of input
        horizontalInput = input.x;
        verticalInput = input.y;
        
        movement = new Vector2(horizontalInput, verticalInput);
        
        /*
        // Normalize diagonal movement
        if (movement.magnitude > 1)
        {
            
        }
        */
        
        movement.Normalize();
        rbPlayer.linearVelocity = movement * moveSpeed;
        
        // Clamp position to gameplay area
        /*
        playerPosition = rbPlayer.position;
        playerPosition.x = Mathf.Clamp(playerPosition.x, minX, maxX);
        playerPosition.y = Mathf.Clamp(playerPosition.y, minY, maxY);
        rbPlayer.position = playerPosition;
        */
    }

    /// <summary>
    /// Take damage when the player hits a bullet and then delete
    /// the bullet.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player is currently invincible, don't take damage
        if (other.CompareTag("Bullet") && !isInvincible)
        {
            Destroy(other.gameObject);
            TakeDamage();
        }
    }
    
    /// <summary>
    /// Decrease current amount of lives and start invincibility window.
    /// </summary>
    private void TakeDamage()
    {
        Debug.Log("ouch");
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
        gameManager.UpdateLivesUI(currentLives, maxLives);
        
        // Check for game over
        if (currentLives <= 0)
        {
            Die();    
        }

        if (gameObject.activeSelf)
        {
            StartCoroutine(DamageCooldown());
            
        }
    }
    
    /// <summary>
    /// Ends the game when HP reaches 0
    /// </summary>
    private void Die()
    {
        // Trigger game over
        if (gameManager != null)
        {
            gameManager.isGameSuccess(false);
        }
        
        // Optional: Disable player controls or trigger death animation
        this.enabled = false;
        
        // Optional: Destroy player with delay or deactivate
        // Destroy(gameObject, 1f);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Grants player an invincibility window when they get hit, and also turns them
    /// red. Will decrease speed slightly.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DamageCooldown()
    {
        spriteRendererPlayer.color = new Color(Color.red.r, Color.red.g, Color.red.b, invincibleAlphaVal);
        isInvincible = true;
        
        // Slow down the player
        var tmp = moveSpeed;
        moveSpeed = hurtSpeed;
        
        yield return new WaitForSeconds(2.0f);
        spriteRendererPlayer.color = colorPlayer;
        
        // Resume normal speed
        isInvincible = false;
        moveSpeed = tmp;
    }
}