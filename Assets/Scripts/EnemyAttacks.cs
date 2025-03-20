using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy attacks", fileName = "New enemy attacks")]
public class EnemyAttacks : ScriptableObject
{
    public List<Bullet> Attacks { get => attacks; }
    [SerializeField] private List<Bullet> attacks = new();
}
