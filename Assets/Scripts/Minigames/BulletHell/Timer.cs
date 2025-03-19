using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    
    private float duration;         // The duration of the timer
    private float maxDuration;
    public UnityEvent OnTimerEnd;   // The event invoked when the timer goes to 0
    private Action timerTick;

    //private BulletSpawner[] spawners;

    /// <summary>
    /// Starts the timer at a given time.
    /// </summary>
    /// <param name="time"></param>
    public void StartTimer(float time)
    {
        if (time == 0f)
        {
            // Send to done method
            OnTimerEnd?.Invoke();
        }
        duration = time;
        maxDuration = duration;
        StartCoroutine(DoTimer());
    }

    private void OnEnable()
    {
        timerText.text = "00.00";
    }

    private void Awake()
    {
        // Get the list of spawners in the scene
        var spawners = FindObjectsOfType<BulletSpawner>();

        foreach (var spawner in spawners)
        {
            timerTick += () => spawner.IncreaseSpeed();
        }
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void PauseTimer()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        DecreaseTimer();
    }

    private void DecreaseTimer()
    {
        if (duration <= 0f) return;
        
        duration -= Time.deltaTime;
    }

    /// <summary>
    /// Decrements timer by 0.01 every 0.01 second until the timer is 0, then invokes
    /// FinishTimer Unity event.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoTimer()
    {
        while (duration > 0)
        {
            // while the timer is below the halfway point, keep increasing difficulty
            if (duration <= maxDuration / 2)
            {
                timerTick?.Invoke();
            }

            timerText.text = duration.ToString("F2");
            yield return new WaitForSeconds(0.01f);            
        }
        FinishTimer();
    }

    /// <summary>
    /// Invokes a game end event when timer finishes.
    /// </summary>
    private void FinishTimer()
    {
        timerText.text = "00.00";   // Reset timer
        OnTimerEnd?.Invoke();
    }
}
