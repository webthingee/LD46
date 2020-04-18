using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindHighestValue : MonoBehaviour
{
    public List<Card> CompareLists()
    {
        CardsInUse[] decks = FindObjectsOfType<CardsInUse>().ToArray();

        if (decks.Length <= 0) return null;
        
        List<Card> cardsInPlay = new List<Card>();
        foreach (CardsInUse deck in decks)
        {
            List<Card> cards = deck.allCardsInUse.Where(x => x.cardLocation == CardLocations.inAction).ToList();
            cardsInPlay.AddRange(cards);
        }

        if (cardsInPlay.Count <= 0) return null;
        
        return cardsInPlay.OrderBy(o => o.cardInfo.cardValue).ToList();
    }
    
    public Card HighestValueCard()
    {
        return CompareLists().Last();
    }

}
