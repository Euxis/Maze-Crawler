using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text hpText;

    private int health;
    
    
    public void DeductHealth(int p)
    {
        health -= p;
        UpdateUI();
    }

    public void SetHealth(int p)
    {
        health = p;
        UpdateUI();
    }

    public int GetHealth()
    {
        return health;
    }

    public bool IsHealthZero()
    {
        return health == 0;
    }

    public void UpdateUI()
    {
        hpText.text = health.ToString();
    }

    public void SetTextColor(Color color)
    {
        hpText.color = color;
    }
}
