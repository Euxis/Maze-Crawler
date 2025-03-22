using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Normal bullet class: moves forward at constant speed
/// </summary>
public class Bullet : Attack
{
    public float AdditionalSpeed { get; set; }
    [SerializeField] private float initialSpeed;
    
    [Header("Visual Effects")]
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Color bulletColor = Color.red;
    
    private SpriteRenderer spriteRenderer;

    protected override void Setup()
    {
        if (TryGetComponent(out spriteRenderer))
        {
            spriteRenderer.color = bulletColor;
        }
    }

    public override void Launch(Vector2 dir)
    {
        dir = dir.normalized;
        rb.linearVelocity = dir * (initialSpeed + AdditionalSpeed);
    }
    
    protected void OnBecameInvisible()
    {
        // Destroy bullet when it goes offscreen
        Destroy(gameObject);
    }
}