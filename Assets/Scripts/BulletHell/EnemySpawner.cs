using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * By Kenjiro Mai
 * 2025
 */

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private PrefabLoader enemyLoader;
    [SerializeField] private EnemyManager enemyManager;
    private List<string> allEnemyIds = new();
    private const int MAX_RANDOM_FAILED_SPAWN_ATTEMPTS = 10;

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
        
        var newEnemy = enemyLoader.SpawnPrefab(randomEnemyId, position);

        if (!newEnemy.TryGetComponent<EnemyBase>(out var e))
        {
            Debug.LogError($"({name}) No EnemyBase found on {randomEnemyId}");
            return;
        }
        
        enemyManager.AddEnemy(e);
    }

    /// <summary>
    /// Spawn multiple random enemies over a given area
    /// </summary>
    /// <param name="amount">Number of enemies to spawn</param>
    /// <param name="lowerBound">Lower bound of area to spawn enemies in</param>
    /// <param name="upperBound">Upper bound of area to spawn enemies in</param>
    /// <param name="positionCheck">Predicate for valid positions; 
    /// return <b>true</b> if valid, <b>false</b> if invalid.</param>
    public void SpawnRandomEnemies(int amount, Vector2 lowerBound,
        Vector2 upperBound, Func<Vector2, bool> positionCheck = null)
    {
        // Do nothing if invalid amount given
        if (amount <= 0) return;

        var attempts = 0;
        var failed = 0;

        while (attempts < amount && failed < MAX_RANDOM_FAILED_SPAWN_ATTEMPTS)
        {
            // Choose a random position between the bounds
            var randomPosition = new Vector2(
                Random.Range(lowerBound.x, upperBound.x),
                Random.Range(lowerBound.y, upperBound.y));

            // Attempt to invoke positionCheck. If it returns false,
            // do not spawn an enemy because it's an invalid position
            // and try again.
            // If positionCheck returns true or if it's null, spawn
            // an enemy in the given position.
            if (!positionCheck?.Invoke(randomPosition) ?? false)
            {
                failed++;
                continue;
            }

            SpawnRandomEnemy(randomPosition);
            attempts++;
        }
    }
}
