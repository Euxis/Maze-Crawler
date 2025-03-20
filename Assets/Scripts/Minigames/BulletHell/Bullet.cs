using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Base class for bullets
/// </summary>
public class Bullet : MonoBehaviour
{
    [SerializeField] private float initialSpeed;
    
    [Header("Bullet Lifetime")]
    [SerializeField]
    private float lifetimeSeconds = 5f;
    
    [Header("Visual Effects")]
    public TrailRenderer trail;
    public Color bulletColor = Color.red;
    
    [SerializeField] private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

      // Awake is called when the script is initialized
    protected void Awake()
    {
        if (TryGetComponent(out spriteRenderer))
        {
            spriteRenderer.color = bulletColor;
        }
        
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetimeSeconds);
    }
    
    /*
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        
        // Optional: Rotate sprite to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    */
    
    /*
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    */

    /*
    public void Launch()
    {
        rb.linearVelocity = direction * speed;
    }
    */

    public virtual void Launch(Vector2 dir)
    {
        dir = dir.normalized;
        rb.linearVelocity = dir * initialSpeed;
    }
    
    protected void OnBecameInvisible()
    {
        // Destroy bullet when it goes offscreen
        Destroy(gameObject);
    }
}