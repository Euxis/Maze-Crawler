using System.Collections.Generic;
using UnityEngine;

/*
 * By Kenjiro Mai
 * 2025
 */

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private PrefabLoader enemyLoader;
    private List<string> allEnemyIds = new();

    private void Start()
    {
        // Preload enemy ids (this assumes that enemyLoader
        // doesn't load new enemies after Awake())
        allEnemyIds = enemyLoader.ObjectIds;
    }
    
    /// <summary>
    /// Spawn a random enemy at given position
    /// </summary>
    /// <param name="position"></param>
    public void SpawnRandomEnemy(Vector2 position)
    {
        var random = Random.Range(0, allEnemyIds.Count);
        var randomEnemyId = allEnemyIds[random];
        
        enemyLoader.SpawnPrefab(randomEnemyId, position);
    }

    /// <summary>
    /// Spawn multiple random enemies over a given area
    /// </summary>
    /// <param name="amount">Number of enemies to spawn</param>
    /// <param name="lowerBound">Lower bound of area to spawn enemies in</param>
    /// <param name="upperBound">Upper bound of area to spawn enemies in</param>
    public void SpawnRandomEnemies(int amount, Vector2 lowerBound,
        Vector2 upperBound)
    {
        // Do nothing if invalid amount given
        if (amount <= 0) return;
        
        var randomPosition = Vector2.zero;
        
        for (var i = 0; i < amount; i++)
        {
            randomPosition = new Vector2(
                Random.Range(lowerBound.x, upperBound.x),
                Random.Range(lowerBound.y, upperBound.y));
            
            SpawnRandomEnemy(randomPosition);
        }
    }
}
