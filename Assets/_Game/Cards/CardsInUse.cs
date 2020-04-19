using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardsInUse : MonoBehaviour
{
    public int cardsToDeal = 1;
    public List<Card> allCardsInUse = new List<Card>();
    public CardLayout hand;
    public CardLayout actionZone;
    public Deck deck;
    public bool isFish;

    public void Deal(int numToDeal)
    {
        StartCoroutine(DealWithTime(numToDeal));
    }

    IEnumerator DealWithTime(int numToDeal)
    {
        for (int i = 0; i < numToDeal; i++)
        {
            
            if (deck.cardsInDrawPile.Count == 0)
            {
                Debug.Log("Shuffle Discard into Draw Pile");
                deck.cardsInDrawPile = deck.cardsInDiscardPile.OrderBy(rand => Random.value).ToList();
                deck.cardsInDiscardPile.Clear();
            }
            
            deck.cardsInDrawPile.First().cardInfo.handInUse = this;
            yield return StartCoroutine(deck.cardsInDrawPile.First().Draw());
        }
    }
    
    #region ### Card Utilities

    public void Discard(Card cardToDiscard)
    {
        Card card = HideCard(allCardsInUse, cardToDiscard);
        if (card != null)
        {
            card.cardInfo.handInUse = null;
            allCardsInUse.Remove(card);
            deck.cardsInDiscardPile.Add(card);
            deck.discardPile.MakeChild(card);
            return;
        }

        card = HideCard(deck.cardsInDrawPile, cardToDiscard);
        if (card != null)
        {
            card.cardInfo.handInUse = null;
            deck.cardsInDrawPile.Remove(card);
            deck.cardsInDiscardPile.Add(card);
            deck.discardPile.MakeChild(card);
            return;
        }
        
        if (card == null) Debug.LogWarning("No Card To Discard");
    }

    public void DiscardFromHand()
    {
        foreach (Card card in allCardsInUse.Where(x => x.cardLocation == CardLocations.inHand).ToList())
        {
            card.cardInfo.handInUse = null;
            card.Discard();
        }
    }

    public void Draw(Card cardToDiscard)
    {
        Card card = ShowCard(deck.cardsInDrawPile, cardToDiscard);
        if (card != null)
        {
            card.cardInfo.handInUse = this;
            deck.cardsInDrawPile.Remove(card);
            allCardsInUse.Add(card);
            
            card.transform.position = transform.position;
            
            Sequence mySequence = DOTween.Sequence();
            mySequence
                .AppendInterval(0.5f)
                .AppendCallback(card.ShowFront)
                .Append(card.transform.DOMove(Vector3.zero, 0.5f))
                .Append(card.transform.DOMove(MoveTo(card).transform.position, 0.5f))
                .AppendInterval(0.1f)
                .OnComplete(() => MoveTo(card).MakeChild(card));
            
            //card.transform.position = transform.position;
            
            
            //hand.MakeChild(card);
            return;
        }

        card = ShowCard(deck.cardsInDiscardPile, cardToDiscard);
        if (card != null)
        {
            card.cardInfo.handInUse = this;
            deck.cardsInDiscardPile.Remove(card);
            allCardsInUse.Add(card);
            hand.MakeChild(card);
            return;
        }
        
        if (card == null) Debug.LogWarning("No Card To Discard");
    }

    public CardLayout MoveTo(Card card)
    {
        if (isFish)
        {
            return actionZone;
        }
        else
        {
            return hand;
        }
    }

    public void ToDrawPile(Card cardToDiscard)
    {
        Card card = HideCard(allCardsInUse, cardToDiscard);
        if (card != null)
        {
            card.cardInfo.handInUse = null;
            allCardsInUse.Remove(card);
            deck.cardsInDrawPile.Add(card);
            deck.drawPile.MakeChild(card);
            return;
        }

        card = HideCard(deck.cardsInDiscardPile, cardToDiscard);
        if (card != null)
        {
            card.cardInfo.handInUse = null;
            deck.cardsInDiscardPile.Remove(card);
            deck.cardsInDrawPile.Add(card);
            deck.drawPile.MakeChild(card);
            return;
        }

        if (card == null) Debug.LogWarning("No Card To Discard");
    }

    private Card HideCard(List<Card> cardList, Card cardToDiscard)
    {
        Card card = cardList.Find(x => x == cardToDiscard);

        if (card == null) return null;
        
        card.gameObject.SetActive(false);
        card.transform.position = Vector3.one * 777f;

        return card;
    }

    private Card ShowCard(List<Card> cardList, Card cardToDiscard)
    {
        Card card = cardList.Find(x => x == cardToDiscard);
        
        if (card == null) return null;

        card.transform.position = Vector3.one; 
        card.gameObject.SetActive(true);

        return card;
    }
    #endregion

    #region ### Deck Utilities
    public int NumCardsInAction()
    {
        return allCardsInUse.Where(x => x.cardLocation == CardLocations.inAction).ToList().Count;
    }

    public List<Card> CardsInAction()
    {
        return allCardsInUse.Where(x => x.cardLocation == CardLocations.inAction).ToList();
    }

    public List<Card> CardsInHand()
    {
        return allCardsInUse.Where(x => x.cardLocation == CardLocations.inHand).ToList();
    }
    
    private List<Card> Shuffle(List<Card> fromDeck)
    {
        return fromDeck.OrderBy(rand => Random.value).ToList();
    }
    #endregion
}
