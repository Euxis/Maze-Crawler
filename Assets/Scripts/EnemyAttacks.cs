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
        // This does look a bit confusing so I'll try to explain
        return attacks
                
            // Select: transform each GameObject in the list to
            // the Attack component attached to that GameObject.
            // Essentially turn the list of GameObjects to a list
            // of Attack components.
            .Select(a => a.GetComponent<Attack>())
            
            // Where: filter out the list of Attacks to only include
            // those that are not null (Unity components can be
            // implicitly converted to booleans, where TRUE is not
            // null).
            .Where(a => a)
            
            // ToList: I lied before. When using LINQ methods
            // (in this case Select() and Where()), the list is
            // converted to a IEnumerable. We convert it back to
            // a list here.
            .ToList();
    }
}
