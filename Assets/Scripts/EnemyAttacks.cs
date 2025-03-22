using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy attacks",
    fileName = "New enemy attacks")]
public class EnemyAttacks : ScriptableObject
{
    public List<Attack> Attacks
    {
        get
        {
            if (actualAttacks == null || actualAttacks.Count == 0)
            {
                actualAttacks = FindAttacks();
            }
            return actualAttacks;
        }
    }

    [SerializeField] private List<GameObject> attacks = new();
    private List<Attack> actualAttacks = new();

    private List<Attack> FindAttacks()
    {
        return attacks
            .Where(a => a.TryGetComponent<Attack>(out _))
            .Select(a => a.GetComponent<Attack>())
            .ToList();
    }
}
