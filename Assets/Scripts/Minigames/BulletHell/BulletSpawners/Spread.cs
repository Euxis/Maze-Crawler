using System;
using UnityEngine;

public class Spread : EnemyBase
{
    [Header("Spread settings")] 
    
    [SerializeField]
    [Tooltip("Angle between bullets of spread shot in degrees")]
    private float spreadAngleDegrees = 30f;

    [SerializeField] private float fireRateSeconds = 1.0f;

    private Attack mainAttack;

    protected override void Setup()
    {
        mainAttack = GetAttackPrefab("normal");
    }

    protected override void Attack(Attack attackPrefab,
        Vector2 direction, Vector2 origin)
    {
        // Instantiate attacks
        var attack1 = InstantiateAttack(attackPrefab, origin);
        var attack2 = InstantiateAttack(attackPrefab, origin);
        var attack3 = InstantiateAttack(attackPrefab, origin);
        
        // Calculate offset angles for attacks 2 and 3
        var attackAngle = Mathf.Atan2(direction.y, direction.x);
        var attackAngle2 = attackAngle + spreadAngleDegrees * Mathf.Deg2Rad;
        var attackAngle3 = attackAngle - spreadAngleDegrees * Mathf.Deg2Rad;
        var attackAngle2Vec2 = new Vector2(
            Mathf.Cos(attackAngle2),
            Mathf.Sin(attackAngle2));
        var attackAngle3Vec2 = new Vector2(
            Mathf.Cos(attackAngle3),
            Mathf.Sin(attackAngle3));
        
        // Launch
        attack1.Launch(direction);
        attack2.Launch(attackAngle2Vec2);
        attack3.Launch(attackAngle3Vec2);
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
