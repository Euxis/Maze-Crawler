using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementSideScrolling : MonoBehaviour
{
    [SerializeField] private GameObject objPlayer;
    private Rigidbody2D rbPlayer;
    private const float moveSpeed = 7.0f;
    private const float jumpSpeed = 13.0f;
    private const float maxJumpSpeed = 10.0f;
    private const float maxMoveSpeed = 250.0f;
    [SerializeField] private Vector2 playerVelocity;

    private float inputVelocityX, inputVelocityY;
    
    [SerializeField] private bool isLanded = false;
    private float raycastDistance = 0.1f;
    
    private void Start()
    {
        rbPlayer = objPlayer.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        playerVelocity = rbPlayer.linearVelocity;
        CheckIfLanded();
    }
    

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            rbPlayer.linearVelocityX = 0;
        }
        if(Mathf.Abs(rbPlayer.linearVelocity.x) >= maxMoveSpeed){
            rbPlayer.linearVelocityX = Mathf.Sign(rbPlayer.linearVelocity.x) * maxMoveSpeed;
        }
        
        rbPlayer.linearVelocityX = input.x * moveSpeed;
        //rbPlayer.AddForce(input * moveSpeed, ForceMode2D.Force);
        //rbPlayer.linearVelocity = (Vector2)input * moveSpeed;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        bool jumpPressed = context.ReadValueAsButton();
        
        
        if (isLanded && jumpPressed)
        {
            isLanded = false;
            rbPlayer.linearVelocityY = jumpSpeed;
            
            //rbPlayer.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        }
        else if(context.canceled && !isLanded)
        {
            rbPlayer.linearVelocityY = 0;
   
        }
    }
    
    // Check below player object to see if they're grounded
    private bool CheckIfLanded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            objPlayer.transform.position, 
            objPlayer.transform.localScale, 
            0f, 
            Vector2.down, 
            raycastDistance, 
            LayerMask.GetMask("Wall")
            );

        if (hit)
            return isLanded = true;
        else
            return isLanded = false;
    }
}
