using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Card cardPrefab;
    
    public List<CardData> gameDeck = new List<CardData>();
        
    public List<Card> cardsInDrawPile = new List<Card>();
    public CardLayout drawPile;
        
    public List<Card> cardsInDiscardPile = new List<Card>();
    public CardLayout discardPile;
    
    private void Start()
    {
        StartingShuffledPile();
    }
    private void StartingShuffledPile()
    {
        cardsInDrawPile.Clear();

        foreach (CardData cardData in gameDeck)
        {
            // create an instance so we don't mess with the core data
            CardData instanceOfCardData = Instantiate(cardData);
            instanceOfCardData.name = cardData.name;
                
            // create a card gameobject
            Card card = Instantiate(cardPrefab, Vector3.one * 777, Quaternion.identity, drawPile.transform);
            // Init and setup the card as needed
            card.Init(instanceOfCardData, this);
            // Add it to the draw pile
            cardsInDrawPile.Add(card);
            // hide it
            card.gameObject.SetActive(false);
        }

        // shuffle the draw pile
        cardsInDrawPile = Shuffle(cardsInDrawPile);
    }
    
    private List<Card> Shuffle(List<Card> fromDeck)
    {
        return fromDeck.OrderBy(rand => Random.value).ToList();
    }
}
