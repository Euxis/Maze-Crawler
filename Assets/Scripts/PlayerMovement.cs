using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject objPlayer;
    [SerializeField] private MazeGenerate mazeScript;

    private Vector2 nextPosition;
    [SerializeField] private Vector2 gridPosition;

    public void CheckStartPoint()
    {
        Debug.Log(mazeScript.startPoint.ToString());
        gridPosition = mazeScript.startPoint;
        objPlayer.transform.position = mazeScript.startPoint;
        nextPosition = objPlayer.transform.position;
    }

    void Update()
    {
        InterpolateMovement();
    }

    private void InterpolateMovement()
    {
        objPlayer.transform.position = Vector2.Lerp(objPlayer.transform.position, gridPosition, 0.5f);
    }

    // Move using callback context
    public void Movement(InputAction.CallbackContext context)
    {
        // Read the Vector2 value from the context
        Vector2 contextValue = context.ReadValue<Vector2>();
        
        // move up/down/left/right based on the vector2
        if (context.performed)
        {
            var obstacle = Physics2D.OverlapPoint(
                (Vector2)gridPosition + contextValue,
                LayerMask.GetMask("Wall") // Replace this with tag of wall
            );

            if (obstacle)
            {
                return;
            }
            
            // No diagonal movement, detect if x or y is more/less than 0.
            if (contextValue.x > 0 || contextValue.x < 0)
            {
                // Move the player a set distance right/left
                gridPosition += Vector2.right * (contextValue.x/Mathf.Abs(contextValue.x));
                //nextPosition = (Vector2)objPlayer.transform.position + Vector2.right * (contextValue.x / Mathf.Abs(contextValue.x));

                //objPlayer.transform.position = (Vector2)objPlayer.transform.position + Vector2.right * (contextValue.x / Mathf.Abs(contextValue.x));
            }

            if (contextValue.y > 0 || contextValue.y < 0)
            {
                // Move the player a set distance up/down
                gridPosition += Vector2.up * (contextValue.y/Mathf.Abs(contextValue.y));
                //nextPosition = (Vector2)objPlayer.transform.position + Vector2.up * (contextValue.y / Mathf.Abs(contextValue.y));

                //objPlayer.transform.position = (Vector2)objPlayer.transform.position + Vector2.up * (contextValue.y / Mathf.Abs(contextValue.y));
            }
        }
    }
}
