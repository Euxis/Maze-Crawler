using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ByteManager : MonoBehaviour
{
    private int maxBytes = 0;       // Max number of bytes to collect
    private int currentBytes;       // Current bytes collected

    // The UI elements of max and current amount of bytes
    [SerializeField] private TMP_Text currentBytesText;
    [SerializeField] private TMP_Text maxBytesText;

    public UnityEvent onGameComplete;

    private void Awake()
    {
        
    }

    public void SetMaxBytes(int i)
    {
        // Reset byte count
        currentBytes = 0;
        currentBytesText.text = currentBytes.ToString();
        maxBytes = i;
        maxBytesText.text = maxBytes.ToString();
    }

    public void CollectByte()
    {
        // Catch if maxBytes hasn't been set yet
        if (maxBytes == 0) Debug.LogWarning("Max bytes not set!");
        currentBytes++;
        
        currentBytesText.text = currentBytes.ToString();
        
        // If current number of bytes geq max bytes, then invoke game complete 
        if (currentBytes >= maxBytes)
        {
            onGameComplete?.Invoke();
        }
    }
}
