using System;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    public void UpdateText(TMP_Text text, String s)
    {
        text.text = s;
    }
}
