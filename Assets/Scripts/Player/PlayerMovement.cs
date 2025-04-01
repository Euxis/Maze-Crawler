using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject objPlayer;
    
    [Header("Scripts")]
    [SerializeField] private MazeGenerate mazeScript;
    [SerializeField] private SoundManager soundManager;

    private Vector2 lastPosition;
    private Vector2 gridPosition;       // stores player position according to grid
    private bool canInterp = true;
    
    /// <summary>
    /// An event to be invoked when the maze is done generating.
    /// Gets the starting point from MazeGenerate script and sets the player's
    /// starting point to it.
    /// </summary>
    public void CheckStartPoint()
    {
        gridPosition = mazeScript.startPoint;
        objPlayer.transform.position = mazeScript.startPoint;
    }

    /// <summary>
    /// Returns player to previous position if minigame is lost
    /// </summary>
    public void ReturnLastPosition()
    {
        canInterp = false;
        objPlayer.transform.position = lastPosition;
        canInterp = true;
    }

    void Update()
    {
        if(canInterp) InterpolateMovement();
    }

    // Smoothly moves the player to the next position
    private void InterpolateMovement()
    {
        objPlayer.transform.position = Vector2.Lerp(objPlayer.transform.position, gridPosition, 8f * Time.deltaTime);
    }
    
    /// <summary>
    /// Move using callback context
    /// </summary>
    /// <param name="context"></param>
    public void Movement(InputAction.CallbackContext context)
    {
        // Read the Vector2 value from the context
        Vector2 contextValue = context.ReadValue<Vector2>();
        
        // Move up/down/left/right based on the vector2
        if (context.performed)
        {
            // Check if the next point is obstructed by a wall
            var obstacle = Physics2D.OverlapPoint(
                (Vector2)gridPosition + contextValue/2,
                LayerMask.GetMask("Wall") 
            );
            // Second wall check using boxcast
            var areaObstacle = Physics2D.OverlapBox(
                (Vector2)gridPosition + contextValue/2, 
                new Vector2(0.5f, 0.5f), 
                0f, 
                LayerMask.GetMask("Wall"));
            // If it is, then don't move
            if (areaObstacle || obstacle)
            {
                return;
            }
            
            // This somehow fixes clipping issues
            if (context.canceled)
            {
                return;
            }

            // No diagonal movement, detect if x or y is more/less than 0.
            if (contextValue.x != 0)
            {
                // Move the player a set distance right/left
                lastPosition = gridPosition;
                gridPosition += Vector2.right * (contextValue.x/Mathf.Abs(contextValue.x));
                soundManager.PlayStep();

                //nextPosition = (Vector2)objPlayer.transform.position + Vector2.right * (contextValue.x / Mathf.Abs(contextValue.x));
                //objPlayer.transform.position = (Vector2)objPlayer.transform.position + Vector2.right * (contextValue.x / Mathf.Abs(contextValue.x));
            }

            if (contextValue.y != 0)
            {
                // Move the player a set distance up/down
                lastPosition = gridPosition;

                gridPosition += Vector2.up * (contextValue.y/Mathf.Abs(contextValue.y));
                
                soundManager.PlayStep();
                //nextPosition = (Vector2)objPlayer.transform.position + Vector2.up * (contextValue.y / Mathf.Abs(contextValue.y));
                //objPlayer.transform.position = (Vector2)objPlayer.transform.position + Vector2.up * (contextValue.y / Mathf.Abs(contextValue.y));
            }
        }
    }
}
