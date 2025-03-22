using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sprinkler : EnemyBase
{
    [Header("Sprinkler settings")]
    
    [SerializeField]
    [Tooltip("Angle between each attack round as it rotates.")]
    private float angleBetweenRoundsDegrees = 10f;

    [SerializeField] 
    [Tooltip("Time between each round in seconds")]
    private float fireRateSeconds = 1.0f;
    
    private float currentRotationDegrees = 0f;
    private Attack mainAttack;
    private int randomDirection = 1;

    protected override void Setup()
    {
        mainAttack = GetAttackPrefab("normal");
        randomDirection = Mathf.RoundToInt(
            Mathf.Pow(-1, Random.Range(0, 2)));
    }

    protected override void Attack(Attack attackPrefab, Vector2 
            direction, Vector2 origin)
    {
        var attack1 = InstantiateAttack(attackPrefab, origin);
        var attack2 = InstantiateAttack(attackPrefab, origin);
        
        attack1.Launch(direction);
        attack2.Launch(direction * -1);
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

        Action attackAction = () =>
        {
            var rotationRad = currentRotationDegrees * Mathf.Deg2Rad;
            var rotationVec2 = new Vector2(
                Mathf.Cos(rotationRad),
                Mathf.Sin(rotationRad));
            Attack(mainAttack, rotationVec2, transform.position);
            currentRotationDegrees += 
                randomDirection * angleBetweenRoundsDegrees;
            currentRotationDegrees %= 360f;
        };
        
        StartCoroutine(AttackCycle(fireRateSeconds, attackAction));
    }
}
