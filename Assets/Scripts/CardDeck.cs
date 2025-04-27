using UnityEngine;
using System.Linq;
using System.Collections.Generic;



public class CardDeck : ScriptableObject
{
    public List<Card> Cards
    {
        get
        {
            if (cardList == null || cardList.Count == 0)
            {
                cardList = FindCards();
            }
            return cardList;
        }
    }

    [SerializeField] private List<Card> cards = new();
    private List<Card> cardList = new();

    private List<Card> FindCards()
    {
        return cards
            //.Select(a => a.GetComponent<Card>())
            .Where(a => a )
            .ToList();
    }
}
