using System;
using System.Collections;
using UnityEngine;

public class SlowFieldEnemy : EnemyBase
{
    [Header("Parameters")] [SerializeField]
    private float fireRateSeconds;

    private Attack mainAttack;

    protected override void Setup()
    {
        mainAttack = GetAttackPrefab("slowfield");
    }

    protected override void Attack(Attack bulletPrefab, Vector2 direction, Vector2 origin)
    {
        StartCoroutine(Fire(fireRateSeconds, bulletPrefab, direction, origin));
    }
    
    private IEnumerator Fire(float delay, Attack prefab, Vector2 direction, Vector2 origin)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(prefab, gameObject.transform);
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
