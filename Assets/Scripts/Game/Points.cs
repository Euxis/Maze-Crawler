using System;
using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    // Get reference to points UI text
    [SerializeField] private TMP_Text points;
    [SerializeField] private TMP_Text HP;
    [SerializeField] private TMP_Text gameOver;
    private int pointCount;
    private int HPCount;

    [SerializeField] private GameObject objPlayer;
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        gameOver.gameObject.SetActive(false);
        HPCount = 3;
        pointCount = 0;
    }

    // Checks state of game. If the player has 0 lives left, game over.
    // If they have sufficient number of points, they win!
    private void StateCheck()
    {
        if (HPCount == 0)
        {
            // Send game over event
            gameOver.gameObject.SetActive(true);
        }

        if (HPCount == 2)
        {
            HP.color = Color.yellow;
            objPlayer.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (HPCount == 1)
        {
            HP.color = Color.red;
            objPlayer.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void AddPoints(int p)
    {
        pointCount += p;
        points.text = pointCount.ToString();
    }

    public void RemoveLife()
    {
        HPCount--;
        StateCheck();
        playerMovement.ReturnLastPosition();
        HP.text = HPCount.ToString();
    }

}
