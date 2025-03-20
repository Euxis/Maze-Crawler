using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{
    /// <summary>
    /// List of all loaded prefabs' objectIds
    /// </summary>
    public List<string> ObjectIds
    {
        get => prefabs.Select(x => x.ObjectId).ToList();
    }
    
    [SerializeField] 
    [Tooltip("The path where prefabs are located. Must be under " +
             "Resources directory. <b>Only include directories " +
             "after Resources/</b>.")]
    private string prefabPath;
    private List<SpawnableObject> prefabs = new();

    private void Awake()
    {
        prefabs = LoadPrefabs(prefabPath);
    }

    private List<SpawnableObject> LoadPrefabs(string path)
    {
        var temp = new List<SpawnableObject>();
        
        // Check validity of path
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"({name}) prefabPath is empty");
            enabled = false;
            return temp;
        }
        
        // Load prefabs from prefabPath
        temp = Resources.LoadAll<SpawnableObject>(path).ToList();
        
        // Check for and remove all duplicates
        temp.ForEach(p =>
        {
            var count = prefabs
                .Count(x => x.ObjectId == p.ObjectId);
            
            // If there are more than one SpawnableObject with the
            // current ones ObjectId, remove all
            if (count > 1)
            {
                temp.RemoveAll(x => 
                    x.ObjectId == p.ObjectId);
                Debug.LogWarning($"({name}) found multiple instances " +
                                 $"of {p.ObjectId}. Removed all.");
            }
        });

        return temp;
    }

    /// <summary>
    /// Get prefab with given objectId
    /// </summary>
    /// <param name="objectId">Unique identifier for prefab</param>
    /// <returns>Prefab with given objectId</returns>
    public SpawnableObject GetPrefab(string objectId)
    {
        var exists = prefabs
            .Any(x => x.ObjectId == objectId);

        if (!exists)
        {
            Debug.LogError($"({name}) prefab not found: {objectId}");
            return null;
        }
        
        return prefabs.FirstOrDefault(
            x => x.ObjectId == objectId);
    }

    public bool TryGetPrefab(string objectId, out SpawnableObject prefab)
    {
        prefab = GetPrefab(objectId);
        return prefab != null;
    }

    /// <summary>
    /// Spawn a prefab with given objectId at given position and
    /// in given rotation
    /// </summary>
    /// <param name="objectId">Unique identifier for prefab</param>
    /// <param name="position">Position to spawn prefab at</param>
    /// <param name="rotation">Rotation to spawn prefab in</param>
    /// <returns>Newly instantiated prefab</returns>
    public GameObject SpawnPrefab(string objectId,
        Vector3 position,
        Quaternion rotation)
    {
        if (!TryGetPrefab(objectId, out var prefab))
        {
            return null;
        }
        
        return Instantiate(prefab.gameObject, position, rotation);
    }
    
    /// <summary>
    /// Spawn a prefab with given objectId at given position and in
    /// identity rotation (0 degrees)
    /// </summary>
    /// <param name="objectId">Unique identifier for prefab</param>
    /// <param name="position">Position to spawn prefab at</param>
    /// <returns>Newly instantiated prefab</returns>
    public GameObject SpawnPrefab(string objectId,
        Vector3 position)
    {
        return SpawnPrefab(objectId, position, Quaternion.identity);
    }
}
