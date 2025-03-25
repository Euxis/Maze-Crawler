using Unity.VisualScripting;
using UnityEngine;

public class HomingBullet : Attack
{
    public float AdditionalSpeed { get; set; }
    [SerializeField] private float initialSpeed;
    
    private SpriteRenderer spriteRenderer;
    
    // The bullet will turn yellow when homing onto the player
    [SerializeField] private Color normalColor = Color.red;
    [SerializeField] private Color homingColor = Color.yellow;
    [SerializeField] private TrailRenderer trailRenderer;
    // Max turning speed for homing. The higher, the more accurate the bullet is.
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private HomingDetector homingDetector;
    
    protected override void Setup()
    {
        if(TryGetComponent(out spriteRenderer)){
            spriteRenderer.color = normalColor;
        }
    }

    /// <summary>
    /// Will launch as normal, homes in on player if they are nearby.
    /// </summary>
    /// <param name="dir">The direction where the bullet will goe</param>
    public override void Launch(Vector2 dir)
    {
        dir = dir.normalized;
        rb.linearVelocity = dir * (initialSpeed + AdditionalSpeed);
    }
    

    public void FollowTarget(GameObject target)
    {
        if(target==null)return;
        
        spriteRenderer.color = homingColor;
        trailRenderer.startColor = homingColor;
        
        // Correct direction vector (toward the player)
        Vector2 direction = (target.transform.position - transform.position).normalized;

        // Calculate angle toward the player
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate smoothly toward the target
        float newRotation = Mathf.LerpAngle(rb.rotation, targetAngle, Time.deltaTime * maxTurnSpeed);
        rb.rotation = newRotation;

        // Move in the direction the bullet is facing
        rb.linearVelocity = new Vector2(Mathf.Cos(newRotation * Mathf.Deg2Rad), Mathf.Sin(newRotation * Mathf.Deg2Rad)) * rb.linearVelocity.magnitude;

    }

    /// <summary>
    /// Destroys self when offscreen
    /// </summary>
    protected void OnBecomeInvisible()
    {
        Destroy(gameObject);
    }
}
