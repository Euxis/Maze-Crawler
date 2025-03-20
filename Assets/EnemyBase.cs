using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected EnemyAttacks attacks;
    
    /// <summary>
    /// Where bullets are spawned. Defaults to this GameObject's
    /// parent
    /// </summary>
    protected Transform localSpace;
    protected GameObject target;

    protected void Awake()
    {
        localSpace = transform.parent;
    }
    
    /// <summary>
    /// Spawns a bullet and sets its properties
    /// </summary>
    /// <param name="direction">Direction that the bullet will travel</param>
    /// <param name="origin">Where the bullet will be spawned</param>
    protected void FireBullet(Bullet bulletPrefab, Vector2 
        direction, Vector2 origin)
    {
        var bullet = Instantiate(bulletPrefab, origin, Quaternion
            .identity, localSpace);
        bullet.Launch(direction);
    }
}
