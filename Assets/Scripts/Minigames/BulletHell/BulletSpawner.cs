// BulletSpawner.cs
using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    
    [Header("Spawn Settings")]
    public float spawnRate = 0.5f;
    [SerializeField] private bool randomizeDirection = true;
    public Vector2 fixedDirection = Vector2.down;
    
    [Header("Pattern Settings")]
    [SerializeField] private SpawnPattern spawnPattern = SpawnPattern.Single;
    [SerializeField] private int bulletsPerPattern = 5;
    [SerializeField] private float spreadAngle = 30f;

    private bool canSpawn = true;

    // Set GameManager reference here instead of at Start
    //[SerializeField] private GameObject objGameManager;
    //private GameManager gameManager;
    
    public enum SpawnPattern
    {
        Single,
        Line,
        Circle,
        Spiral
    }

    void Awake()
    {
        /*
        GameManager is never used 
         
        gameManager = objGameManager.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("Game manager is null!");
        }*/
    }

    void Start()
    {
        StartCoroutine(SpawnBullets());
    }
    
    IEnumerator SpawnBullets()
    {
        while (canSpawn)
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
            }
            /*
            if (!gameManager.IsGameOver())
            {
                
            }*/
            
            yield return new WaitForSeconds(spawnRate);
        }
    }

    public void StopSpawning()
    {
        canSpawn = false;
    }

    private void SpawnSingleBullet()
    {
        Vector2 direction = randomizeDirection ? Random.insideUnitCircle.normalized : fixedDirection;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(direction);
            bulletComponent.SetSpeed(bulletSpeed);
        }
    }
    
    private void SpawnLineBullets()
    {
        float startAngle = -spreadAngle / 2;
        float angleStep = spreadAngle / (bulletsPerPattern - 1);
        
        for (int i = 0; i < bulletsPerPattern; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * fixedDirection;
            
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            
            bulletComponent.SetDirection(direction);
            bulletComponent.SetSpeed(bulletSpeed);
        }
    }
    
    private void SpawnCircleBullets()
    {
        float angleStep = 360f / bulletsPerPattern;
        
        for (int i = 0; i < bulletsPerPattern; i++)
        {
            float angle = angleStep * i;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            
            bulletComponent.SetDirection(direction);
            bulletComponent.SetSpeed(bulletSpeed);
        }
    }
    
    private void SpawnSpiralBullet()
    {
        // Use Time.time to create a continuous spiral pattern
        float angle = Time.time * 120f; // 120 degrees per second
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        
        bulletComponent.SetDirection(direction);
        bulletComponent.SetSpeed(bulletSpeed);
    }
 
}