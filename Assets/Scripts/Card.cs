using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public string CardId { get => cardId; }
    [SerializeField] protected string cardId;
    [SerializeField] private LocalizedString title;
    [SerializeField] private LocalizedString description;
    private string[] text;
    
    public string[] GetDescAndTitle()
    {
        text[0] = title.GetLocalizedString(title);
        text[1] = description.GetLocalizedString(description);
        return text;
    }

    /// <summary>
    /// Set effects of the card
    /// </summary>
    public virtual void Effect() { }
}
