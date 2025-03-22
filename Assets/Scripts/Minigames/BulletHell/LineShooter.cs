using System;
using System.Collections;
using UnityEngine;
using System.Linq;

public class LineShooter : EnemyBase
{
    [SerializeField] private float fireRateSeconds;
    [SerializeField] private int shotsPerRound;
    [SerializeField] private float delayBetweenShotsSeconds;
    private Attack mainAttack;
    
    protected override void Setup()
    {
        // Get main attack "normal"
        mainAttack = GetAttackPrefab("normal");
    }

    protected override void Attack(Attack bulletPrefab, 
        Vector2 direction, Vector2 origin)
    {
        var currentDelay = 0f;
        for (var i = 0; i < shotsPerRound; i++)
        {
            StartCoroutine(DelayAttack(currentDelay, 
                bulletPrefab, direction, origin));
            currentDelay += delayBetweenShotsSeconds;
        }
    }

    private IEnumerator DelayAttack(float delay, Attack prefab, 
        Vector2 direction, Vector2 origin)
    {
        yield return new WaitForSeconds(delay);
        var attack = InstantiateAttack(prefab, origin);
        attack.Launch(direction);
    }

    public override void StartAttack()
    {
        if (attacking) return;
        
        // Check mainAttack
        if (!mainAttack)
        {
            Debug.LogError($"({name}): mainAttack not found");
            return;
        }
        
        // Check target
        if (!target)
        {
            Debug.LogError($"({name}): target not found");
            return;
        }
        
        attacking = true;

        Action attackAction = () =>
        {
            // Every attack cycle calculate the direction to attack at,
            // then attack in that direction
            var dir = target.transform.position - transform.position;
            Attack(mainAttack, dir, transform.position);
        };
        
        StartCoroutine(AttackCycle(fireRateSeconds, attackAction));
    }
}
