using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class CompareHands : MonoBehaviour
{
    public CardsInUse fateCards;
    public CardsInUse playerCards;
    
    public Meter happyMeter;
    public Meter hungerMeter;
    public Meter dirtyMeter;
    public Meter sanityMeter;
    
    [Button]
    public void ExecutePlayerCards()
    {
        StartCoroutine(ExecuteEachCard());
    }

    IEnumerator ExecuteEachCard()
    {
        foreach (Card card in playerCards.CardsInAction())
        {
            Debug.Log($"execute events for + {card.name}");
            ExecuteCard(card);
            yield return new WaitForSeconds(0.5f);
            
            playerCards.Discard(card);
            Debug.Log($"player discards + {card.name}");

        }
        
        foreach (Card card in fateCards.CardsInAction())
        {
            Debug.Log($"fate discards + {card.name}");
            yield return new WaitForSeconds(0.5f);
            
            fateCards.Discard(card);
        }
        
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<GameStateMaster>().gameState = GameState.GameCleanup;
    }
    
    public void ExecuteCard(Card cardToExecute)
    {
        switch (cardToExecute.cardInfo.cardSuit)
        {
            case Suits.Clubs:
                happyMeter.currentValue += cardToExecute.cardInfo.cardValue;
                break;
            case Suits.Diamonds:
                hungerMeter.currentValue += cardToExecute.cardInfo.cardValue;
                break;
            case Suits.Hearts:
                dirtyMeter.currentValue += cardToExecute.cardInfo.cardValue;
                break;
            case Suits.Spades:
                sanityMeter.currentValue += cardToExecute.cardInfo.cardValue;
                break;
        }

        Debug.Log($"fate played + {cardToExecute.name}");
    }
    
    public void ExecuteFateCard(Card cardToExecute)
    {
        switch (cardToExecute.cardInfo.cardSuit)
        {
            case Suits.Clubs:
                happyMeter.currentValue -= cardToExecute.cardInfo.cardValue;
                break;
            case Suits.Diamonds:
                hungerMeter.currentValue -= cardToExecute.cardInfo.cardValue;
                break;
            case Suits.Hearts:
                dirtyMeter.currentValue -= cardToExecute.cardInfo.cardValue;
                break;
            case Suits.Spades:
                sanityMeter.currentValue -= cardToExecute.cardInfo.cardValue;
                break;
        }

        Debug.Log($"fate played + {cardToExecute.name}");
    }
}