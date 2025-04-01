using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MazeTimer : MonoBehaviour
{
    // This script will deduct points continously as the maze game is active
    
    private ScoreManager scoreManager;
    public int deductAmount;    // the amount of points to deduct per interval
    private int currentScore;

    // Grace period at the beginning of the game until the player gets first minigame
    private bool gracePeriod = true;
    private bool isTimerRunning = false;

    public UnityEvent TimerFinish;
    
    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
    }
    
    // Rules:
    // Don't deduct points until the player moves after finishing a minigame
    // Don't deduct points at the start of the game, where the player has zero points
    
    
    // Automatically disable timer when script is inactive (maze is inactive)
    private void OnDisable()
    {
        isTimerRunning = false;
        PauseTimer();
    }

    public void PauseTimer()
    {
        isTimerRunning = false;
        StopAllCoroutines();
    }

    public void StartTimer()
    {
        
        StartCoroutine(Timer());
    }

    public bool IsTimerZero()
    {
        return currentScore == 0;
    }

    public void RemoveGracePeriod()
    {
        gracePeriod = false;
        if(!gameObject.activeSelf) gameObject.SetActive(true);
    }

    /// <summary>
    /// Will resume the timer when the player takes a step after a minigame
    /// </summary>
    /// <param name="context"></param>
    public void TakeStep(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!gracePeriod && !isTimerRunning)
            {
                StartCoroutine(Timer());
            }
        }
    }

    private IEnumerator Timer()
    {
        while (!scoreManager.IsScoreZero())
        {
            isTimerRunning = true;
            scoreManager.DeductPoints(deductAmount);
            yield return new WaitForSeconds(0.1f);
        }

        isTimerRunning = false;
        TimerFinish?.Invoke();
   }
}
