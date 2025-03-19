using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HintPointer : MonoBehaviour
{
    // This script points to the remaining minigame nodes when there are only 5 left.

    private GameObject[] minigameObjects;
    private int nodeCount;
    [SerializeField] private GameObject pointer;    // gameobject of the pointer
    [SerializeField] private GameObject objPlayer;
    private float radius = 0.3f;    // radius from the player of how far the pointer should go
    //private bool canTrack = false;  // Whether the hint pointer is active or not
    private float minDistance = float.PositiveInfinity;
    [SerializeField]private PrefabMinigame target = null;
    private Vector2 targetPos;

    private void Awake()
    {
        
    }

    private void OnEnable()
    {
        if (FindAnyObjectByType<PrefabMinigame>() == null)
        {
            pointer.SetActive(false);
        }
    }

    private void Start()
    {
        if (FindAnyObjectByType<PrefabMinigame>() == null)
        {
            pointer.SetActive(false);
        }
        else
        {
            pointer.SetActive(true);
            
        }

    }

    private void Update()
    {
        if (pointer.activeSelf)
        {
            var direction = ((Vector2)target.transform.position - (Vector2)objPlayer.transform.position).normalized;
            targetPos = (Vector2)objPlayer.transform.position + direction * radius;
            pointer.transform.position = targetPos;            
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GetMinigameObjects();
            TrackMinigameObjects();
        }
    }

    /// <summary>
    /// Counts all minigame nodes in the maze
    /// </summary>
    public void GetMinigameObjects()
    {
        nodeCount = PrefabMinigame.Nodes.Count;
    }
    
    /// <summary>
    /// Decreases count of minigame objects
    /// </summary>


    /// <summary>
    /// Points the pointer to the nearest minigame node
    /// </summary>
    private void TrackMinigameObjects()
    {
        minDistance = float.PositiveInfinity;
        // find the nearest minigame node
        foreach (var node in PrefabMinigame.Nodes)
        {
            // calculate distance
            var distance = Vector2.Distance(objPlayer.transform.position, node.transform.position);
            if (distance < minDistance)
            {
                //Debug.Log("New Target");
                target = node;
                minDistance = distance;
            }
        }
        

        // point to it
        
        
    }
}
