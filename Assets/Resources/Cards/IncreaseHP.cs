using UnityEngine;

public class IncreaseHP : Card
{
    private PlayerController playerController;
    
    public void Effect()
    {
        playerController = FindObjectOfType<PlayerController>();    
        
        playerController.IncreaseBaseHP(1);
    }
}
