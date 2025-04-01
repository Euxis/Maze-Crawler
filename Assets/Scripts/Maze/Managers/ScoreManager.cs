using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // This script manages the new score system of the game

    private int totalScore = 0;     // The total score of the game

    [Header("UI Elements")]
    [SerializeField] private TMP_Text scoreText;
    
    /// <summary>
    /// Adds points to the score
    /// </summary>
    /// <param name="points">Points to add</param>
    public void AddPoints(int points)
    {
        totalScore += points;
        UpdateText();

    }

    /// <summary>
    /// Sets score to given points
    /// </summary>
    /// <param name="score"></param>
    public void SetScore(int score)
    {
        totalScore = score;
        UpdateText();

    }

    public int GetScore()
    {
        return totalScore;
    }

    /// <summary>
    /// Returns true if the score is zero, false otherwise.
    /// </summary>
    /// <returns></returns>
    public bool IsScoreZero()
    {
        return totalScore == 0;
        
    }

    /// <summary>
    /// Deducts points from the score.
    /// </summary>
    /// <param name="points">Points to deduct</param>
    public void DeductPoints(int points)
    {
        totalScore -= points;
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = totalScore.ToString();
    }
}
