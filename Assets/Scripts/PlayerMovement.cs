using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject objPlayer;
    [SerializeField] private MazeGenerate mazeScript;
    
    
    // TODO:
    // * Make script wait until MazeGenerate has a non (0,0) coordinate for starting point
    void Start()
    {
        
    }

    public void CheckStartPoint()
    {
        Debug.Log(mazeScript.startPoint.ToString());
        objPlayer.transform.position = mazeScript.startPoint;
    }

    // Move using callback context
    public void Movement(InputAction.CallbackContext context)
    {
        // Read the Vector2 value from the context
        Vector2 contextValue = context.ReadValue<Vector2>();
        
        // move up/down/left/right based on the vector2
        if (context.performed)
        {
            // No diagonal movement, detect if x or y is more/less than 0.
            if (contextValue.x > 0 || contextValue.x < 0)
            {
                // Move the player a set distance right/left
                objPlayer.transform.position = (Vector2)objPlayer.transform.position + Vector2.right * (contextValue.x / Mathf.Abs(contextValue.x));
            }

            if (contextValue.y > 0 || contextValue.y < 0)
            {
                // Move the player a set distance up/down
                objPlayer.transform.position = (Vector2)objPlayer.transform.position + Vector2.up * (contextValue.y / Mathf.Abs(contextValue.y));
            }
        }
    }
}
