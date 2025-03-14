using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    
    [Header("Bullet Lifetime")]
    public float lifetime = 5f;
    
    [Header("Visual Effects")]
    public TrailRenderer trail;
    public Color bulletColor = Color.red;
    
    [SerializeField] private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

      // Awake is called when the script is initialized
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = bulletColor;
        }
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }
    
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        
        // Optional: Rotate sprite to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void Launch()
    {
        rb.linearVelocity = direction * speed;
    }

    public void Launch(Vector2 dir, float velocity)
    {
        dir = dir.normalized;
        rb.linearVelocity = dir * velocity;
    }
    
    /*
    void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }
    */
    
    void OnBecameInvisible()
    {
        // Destroy bullet when it goes offscreen
        Destroy(gameObject);
    }
}