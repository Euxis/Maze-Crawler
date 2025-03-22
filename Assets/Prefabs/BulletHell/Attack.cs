using UnityEngine;
using UnityEngine.Serialization;

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
        
        // Destroy bullet after lifetime expires
        Destroy(gameObject, lifetimeSeconds);
    }

    /// <summary>
    /// Run in Awake()
    /// </summary>
    protected virtual void Setup() { }
    
    public virtual void Launch(Vector2 direction) { }
}