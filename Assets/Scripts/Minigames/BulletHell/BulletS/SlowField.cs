using UnityEngine;

public class SlowField : Attack
{
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    // The bullet doesn't need to do anything, movement is done via animation
    public override void Launch(Vector2 dir)
    {
        
    }
}
