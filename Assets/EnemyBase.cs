using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected EnemyAttacks attacks;
    protected bool attacking = false;
    
    /// <summary>
    /// Where bullets are spawned. Defaults to this GameObject's
    /// parent
    /// </summary>
    protected Transform localSpace;
    protected GameObject target;

    protected void Awake()
    {
        localSpace = transform.parent;
        Setup();
    }

    /// <summary>
    /// Sets this enemy's target
    /// </summary>
    /// <param name="target">Target to attack</param>
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    /// <summary>
    /// Start the enemy's assault on target
    /// </summary>
    public virtual void StartAttack() { }

    /// <summary>
    /// Runs in Awake()
    /// </summary>
    protected virtual void Setup() { }
    
    /// <summary>
    /// Fires given bullet in direction
    /// </summary>
    /// <param name="direction">Direction that the bullet will travel</param>
    /// <param name="origin">Where the bullet will be spawned</param>
    protected virtual void Attack(Attack bulletPrefab, Vector2 
        direction, Vector2 origin)
    {
        var bullet = InstantiateAttack(bulletPrefab, 
            origin);
        bullet.Launch(direction);
    }

    /// <summary>
    /// Shorthand for Instantiate. Made a child of localSpace and
    /// rotated to identity Quaternion.
    /// </summary>
    /// <param name="prefab">Attack prefab to instantiate</param>
    /// <param name="direction">Direction of attack</param>
    /// <param name="origin">Origin of attack</param>
    /// <returns>Attack that was instantiated</returns>
    protected Attack InstantiateAttack(Attack prefab,
        Vector2 origin)
    {
        return Instantiate(prefab, origin, Quaternion.identity, 
            localSpace);
    }

    protected IEnumerator AttackCycle(float delaySeconds, 
        Action attackAction)
    {
        attackAction?.Invoke();
        yield return new WaitForSeconds(delaySeconds);
        
        // Start it again
        StartCoroutine(AttackCycle(delaySeconds, attackAction));
    }

    public void StopAttacking()
    {
        StopAllCoroutines();
    }
}
