using System.Collections.Generic;
using UnityEngine;

public class ByteSpawner : MonoBehaviour
{
    private List<Vector2> spawnPoints = new List<Vector2>();
    private int positionX, positionY;

    [SerializeField] private GameObject bytePrefab;
    [SerializeField] private ByteManager byteManager;
    
    private GameObject prefabParent;
    private int numberofBytes;
    
    // Spawn bytes spread throughout scene
    public void MakeBytes()
    {
        prefabParent = GameObject.FindGameObjectWithTag("BulletHellPrefab");
        
        // randomize amount of bytes to spawn
        numberofBytes = Random.Range(3, 6);
        
        // pass max byte amount to byte manager
        byteManager.SetMaxBytes(numberofBytes+1);

        for (int i = 0; i <= numberofBytes; i++)
        {
            RandomizeSpawn();
            Instantiate(bytePrefab, new Vector2(positionX, positionY), Quaternion.identity, prefabParent.transform);
        }

        // clear list when done
        spawnPoints.Clear();
    }

    public void ClearAllBytes()
    {
        var bytesList = GameObject.FindGameObjectsWithTag("BulletHellByte");
        foreach (var bytes in bytesList)
        {
            Destroy(bytes.gameObject);
        }
    }

    /// <summary>
    ///  Returns max number of bytes
    /// </summary>
    private int GetBytes()
    {
        return numberofBytes;
    }

    private void RandomizeSpawn()
    {
        Vector2 newPosition;

        do
        {
            // Get a random spawn point and clamp it to the min and max range
            positionX = Random.Range(-8, 9);
            positionY = Random.Range(-4, 5);

            positionX = Mathf.Clamp(positionX, -8, 8);
            positionY = Mathf.Clamp(positionY, -4, 4);
            newPosition = new Vector2(positionX, positionY);
        } while (spawnPoints.Contains(newPosition) && spawnPoints.Count > 0 && CheckForOccupance(newPosition));
        // double check
        if(CheckForOccupance(newPosition)) spawnPoints.Add(newPosition);
    }

    /// <summary>
    /// Checks the current position if it is occupied by bulletspawner
    /// </summary>
    /// <returns></returns>
    private bool CheckForOccupance(Vector2 pos)
    {
        var checkResults = Physics2D.OverlapCircle(pos, 4f, LayerMask.GetMask("BulletSpawner"));
        // return true if free
        if (checkResults == null)
            return true;
        // false if not
        return false;
    }
}
