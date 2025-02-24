using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerRaycast : MonoBehaviour
{
    // Script to cast a ray from the player based on the direction their are facing

    [SerializeField] private Transform transformPlayer;
    [SerializeField] private PlayerMovement playerMovement;

    public UnityEvent OnWallFace;
    
    
    
    // Variables
    private Vector2 playerDirection;    // The direction that the player is currently facing
    private Vector2 lastDirection;      // The previous direction that the player was facing
    private Vector2 boxSize = new Vector2(0.5f, 0.5f);  // size of boxcast (width)
    private const float boxLength = 2.3f;           // Length of boxcast
    
    private RaycastHit2D boxHit;       // results of boxcast
    
    
    private void Update()
    {
        DrawBoxCast(transformPlayer.position, lastDirection, boxSize, 0f, boxLength);
        GetBoxHit();
    }

    /// <summary>
    /// If the boxcast sees an interactable, tell it to highlight itself
    /// <para>If the player looks away, reset the last interactable highlight</para>
    /// </summary>
    private void GetBoxHit() {
        boxHit = Physics2D.BoxCast(transformPlayer.position, boxSize, 0f, lastDirection, boxLength);
        
            
    }

    /// <summary>
    /// Changes Boxcast direction whenever the player changes direction
    /// </summary>
    /// <param name="context"></param>
    public void SendBoxcast(InputAction.CallbackContext context)
    {
        // read vector from movement input
        playerDirection = context.ReadValue<Vector2>();

        // if the current player direction is DIFFERENT from the last direction faced, but NOT (0,0)
        // then update it
        if(playerDirection != lastDirection && playerDirection.x != 0 || playerDirection.y != 0)
        {
            lastDirection = playerDirection;
        }
    }

    void DrawBoxCast(Vector2 origin, Vector2 direction, Vector2 size, float angle, float distance)
    {
        // Calculate the rotation of the box
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // The four corners of the initial box
        Vector2 topLeft = (Vector2)(rotation * new Vector2(-size.x / 2, size.y / 2)) + origin;
        Vector2 topRight = (Vector2)(rotation * new Vector2(size.x / 2, size.y / 2)) + origin;
        Vector2 bottomLeft = (Vector2)(rotation * new Vector2(-size.x / 2, -size.y / 2)) + origin;
        Vector2 bottomRight = (Vector2)(rotation * new Vector2(size.x / 2, -size.y / 2)) + origin;

        // Offset the box corners by the cast direction and distance
        Vector2 castOffset = direction.normalized * distance;
        Vector2 topLeftEnd = topLeft + castOffset;
        Vector2 topRightEnd = topRight + castOffset;
        Vector2 bottomLeftEnd = bottomLeft + castOffset;
        Vector2 bottomRightEnd = bottomRight + castOffset;

        // Draw the starting box
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.DrawLine(bottomLeft, topLeft, Color.green);

        // Draw the ending box
        Debug.DrawLine(topLeftEnd, topRightEnd, Color.red);
        Debug.DrawLine(topRightEnd, bottomRightEnd, Color.red);
        Debug.DrawLine(bottomRightEnd, bottomLeftEnd, Color.red);
        Debug.DrawLine(bottomLeftEnd, topLeftEnd, Color.red);

        // Draw lines between the start and end positions (to show movement)
        Debug.DrawLine(topLeft, topLeftEnd, Color.yellow);
        Debug.DrawLine(topRight, topRightEnd, Color.yellow);
        Debug.DrawLine(bottomLeft, bottomLeftEnd, Color.yellow);
        Debug.DrawLine(bottomRight, bottomRightEnd, Color.yellow);
    }
}
