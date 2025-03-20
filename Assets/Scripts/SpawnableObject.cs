using UnityEngine;
/*
 * By Kenjiro Mai
 * 2025
 */

/// <summary>
/// Allows PrefabLoader to recognize the GameObject this script is
/// attached to
/// </summary>
public class SpawnableObject : MonoBehaviour
{
    public string ObjectId { get => objectId; }
    [SerializeField] private string objectId;
}
