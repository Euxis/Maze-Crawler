using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyBase> Enemies { get => enemies; }
    private List<EnemyBase> enemies = new();

    public void SetEnemiesTarget(GameObject target)
    {
        if (!target) return;
        
        enemies.ForEach(e => e.SetTarget(target));
    }
    
    public void StartAllEnemies()
    {
        enemies.ForEach(e => e.StartAttack());
    }

    public void StopAllEnemies()
    {
        enemies.ForEach(e => e.StopAttacking());
    }

    public void DestroyAllEnemies()
    {
        enemies.ForEach(e => Destroy(e.gameObject));
    }

    /// <summary>
    /// Set the enemies list to a new list.
    /// </summary>
    /// <param name="newEnemies">New enemies to manage</param>
    /// <returns>Old list of enemies that was replaced</returns>
    public List<EnemyBase> SetEnemies(List<EnemyBase> newEnemies)
    {
        var old = enemies;
        enemies = newEnemies;
        return old;
    }

    public void AddEnemy(EnemyBase newEnemy)
    {
        enemies.Add(newEnemy);
    }

    /// <summary>
    /// Clears list of managed enemies.
    /// </summary>
    /// <returns>List of enemies that was cleared</returns>
    public List<EnemyBase> ClearEnemies()
    {
        var old = enemies;
        enemies.Clear();
        return old;
    }
}
