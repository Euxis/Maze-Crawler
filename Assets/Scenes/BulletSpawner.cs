// BulletSpawner.cs
using System.Collections;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;
    
    [Header("Spawn Settings")]
    public float spawnRate = 0.5f;
    public bool randomizeDirection = true;
    public Vector2 fixedDirection = Vector2.down;
    
    [Header("Pattern Settings")]
    public SpawnPattern spawnPattern = SpawnPattern.Single;
    public int bulletsPerPattern = 5;
    public float spreadAngle = 30f;
    
    private GameManager gameManager;
    
    public enum SpawnPattern
    {
        Single,
        Line,
        Circle,
        Spiral
    }
    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(SpawnBullets());
    }
    
    IEnumerator SpawnBullets()
    {
        while (true)
        {
            if (gameManager != null && !gameManager.IsGameOver())
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
            }
            
            yield return new WaitForSeconds(spawnRate);
        }
    }
    
    void SpawnSingleBullet()
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
    
    void SpawnLineBullets()
    {
        float startAngle = -spreadAngle / 2;
        float angleStep = spreadAngle / (bulletsPerPattern - 1);
        
        for (int i = 0; i < bulletsPerPattern; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, angle) * fixedDirection;
            
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            
            if (bulletComponent != null)
            {
                bulletComponent.SetDirection(direction);
                bulletComponent.SetSpeed(bulletSpeed);
            }
        }
    }
    
    void SpawnCircleBullets()
    {
        float angleStep = 360f / bulletsPerPattern;
        
        for (int i = 0; i < bulletsPerPattern; i++)
        {
            float angle = angleStep * i;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            
            if (bulletComponent != null)
            {
                bulletComponent.SetDirection(direction);
                bulletComponent.SetSpeed(bulletSpeed);
            }
        }
    }
    
    void SpawnSpiralBullet()
    {
        // Use Time.time to create a continuous spiral pattern
        float angle = Time.time * 120f; // 120 degrees per second
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(direction);
            bulletComponent.SetSpeed(bulletSpeed);
        }
    }
 
}