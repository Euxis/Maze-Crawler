using UnityEngine;

/// <summary>
/// Base class for all types of attacks
/// </summary>
public class Attack : MonoBehaviour
{
    public string AttackId { get => attackId; }
    [SerializeField] protected string attackId;
    [SerializeField] protected float lifetimeSeconds;
    [SerializeField] protected Rigidbody2D rb;

    protected void Awake()
    {
        Setup();
        
        lifetimeSeconds = Mathf.Clamp(lifetimeSeconds, 0f, lifetimeSeconds);
        // Destroy bullet after lifetime expires
        Destroy(gameObject, lifetimeSeconds);
    }

    /// <summary>
    /// Run in Awake()
    /// </summary>
    protected virtual void Setup() { }
    
    /// <summary>
    /// Set attack live
    /// </summary>
    /// <param name="direction">Direction of attack</param>
    public virtual void Launch(Vector2 direction) { }
}