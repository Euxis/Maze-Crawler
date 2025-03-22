// BulletSpawner.cs

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletSpawner : MonoBehaviour
{
    [Obsolete]
    
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    
    [Header("Spawn Settings")]
    public float spawnRate = 0.7f;
    [SerializeField] private bool randomizeDirection = true;
    public Vector2 fixedDirection = Vector2.down;

    [SerializeField] private GameObject player;
    
    [Header("Pattern Settings")]
    [SerializeField] private SpawnPattern spawnPattern = SpawnPattern.Single;
    [SerializeField] private int bulletsPerPattern = 3;
    [SerializeField] private float spreadAngle = 30f;
    private int sprinklerStep = 0;

    [SerializeField] private GameObject prefabParent;

    private bool canSpawn = false;
    
    public enum SpawnPattern
    {
        Single,
        Line,
        Circle,
        Spiral,
        Sprinkler
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        prefabParent = GameObject.FindGameObjectWithTag("BulletHellPrefab");
        //StartCoroutine(FindPlayer());
    }

    private IEnumerator FindPlayer()
    {
        yield return new WaitUntil(() => player = GameObject.FindWithTag("Player"));
    }

    public void SetSpawnPattern(int i)
    {
        /*
        switch (i)
        {
            case(0):
                spawnPattern = SpawnPattern.Single;
                break;
            case(1):
                spawnPattern = SpawnPattern.Line;
                break;
            case(2):
                spawnPattern = SpawnPattern.Circle;
                break;
            case(3):
                spawnPattern = SpawnPattern.Spiral;
                break;
            default:
                break;
        }
        */
        
        // Ensure that i is within SpawnPattern values
        i %= Enum.GetNames(typeof(SpawnPattern)).Length;
        SetSpawnPattern((SpawnPattern)i);
    }

    public void SetSpawnPattern(SpawnPattern pattern)
    {
        spawnPattern = pattern;

        if (spawnPattern == SpawnPattern.Sprinkler)
        {
            // fast
            spawnRate = 0.3f;
        }
    }

    private IEnumerator SpawnBullets()
    {
        switch (spawnPattern)
        {
            case SpawnPattern.Single:
                SpawnSingleBullet();
                break;
            case SpawnPattern.Line:
                SpawnLineBullets();
                break;
            case SpawnPattern.Circle:
                SpawnCircleBullets();
                break;
            case SpawnPattern.Spiral:
                SpawnSpiralBullet();
                break;
            case SpawnPattern.Sprinkler:
                SpawnSprinklerBullet(ref sprinklerStep);
                break;
                
        }
        
        yield return new WaitForSeconds(spawnRate);

        if (canSpawn) StartCoroutine(SpawnBullets());
    }
    
    /// <summary>
    /// Starts or stops the bullet spawner from shooting bullets.
    /// </summary>
    /// <param name="b"></param>
    public void StartSpawning(bool b)
    {
        canSpawn = b;
        if(b)
            StartCoroutine(SpawnBullets());
    }

    /// <summary>
    /// Will slightly increase the speed and spawnrate of bullets
    /// </summary>
    public void IncreaseSpeed()
    {
        bulletSpeed += 0.01f;
        spawnRate -= 0.0001f;
    }

    private void SpawnSingleBullet()
    {
        Vector2 direction = randomizeDirection ? Random.insideUnitCircle.normalized : fixedDirection;
        for (int i = 0; i < bulletsPerPattern; i++)
        {
            SpawnAndSetBullet(direction, bulletSpeed - (float)(1 * i), transform.position);
        }
    }
    
    private void SpawnLineBullets()
    {
        if (player == null) {
            Debug.Log("Finding player");
            StartCoroutine(FindPlayer()); // Ensure player reference exists
            return;
        }

        // Get direction from enemy to player
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
    
        // Get the base angle towards the player
        float baseAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
    
        float startAngle = baseAngle - (spreadAngle / 2);
        float angleStep = spreadAngle / (bulletsPerPattern - 1);
    
        for (int i = 0; i < bulletsPerPattern; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right; // Base direction is right
            SpawnAndSetBullet(direction, bulletSpeed, transform.position);
        }

        // Rotate the enemy to face the player
        float enemyRotation = baseAngle + 90f; // Offset to align with sprite forward direction
        transform.rotation = Quaternion.Euler(0, 0, enemyRotation);
    }
    
    private void SpawnCircleBullets()
    {
        float angleStep = 360f / bulletsPerPattern;
        
        // Add some randomness to the angle
        float randomDirectionAddition = Random.Range(0, 91);
        
        for (int i = 0; i < bulletsPerPattern; i++)
        {
            float angle = angleStep * i + randomDirectionAddition;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            SpawnAndSetBullet(direction, bulletSpeed, transform.position);
        }
    }
    
    private void SpawnSpiralBullet()
    {
        // Use Time.time to create a continuous spiral pattern
        float angle = Time.time * 120f; // 120 degrees per second
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        SpawnAndSetBullet(direction, bulletSpeed, transform.position);
    }

    private void SpawnSprinklerBullet(ref int step)
    {
        var angle = step * 30.0f * Mathf.Deg2Rad;
        var angle2 = angle + Mathf.PI;
        var direction = new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle));
        var direction2 = new Vector2(
            Mathf.Cos(angle2),
            Mathf.Sin(angle2));
        SpawnAndSetBullet(direction, bulletSpeed, transform.position);
        SpawnAndSetBullet(direction2, bulletSpeed, transform.position);
        step++;
    }

    /// <summary>
    /// Spawns a bullet and sets its properties
    /// </summary>
    /// <param name="direction">Direction that the bullet will travel</param>
    /// <param name="speed">Speed of the bullet</param>
    /// <param name="origin">Where the bullet will be spawned</param>
    private void SpawnAndSetBullet(Vector2 direction, float speed, Vector2 origin)
    {
        if (prefabParent == null)
        {
            prefabParent = GameObject.FindGameObjectWithTag("BulletHellPrefab");
            return;
        }
        var bullet = Instantiate(bulletPrefab, origin, Quaternion.identity, prefabParent.transform);

        if (bullet.TryGetComponent(out Bullet bulletComponent))
        {
            //bulletComponent.Launch(direction, speed);
        }
    }
 
}