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
            Debug.Log($"player events for + {card.name}");
            ExecuteCard(card);
            
            yield return new WaitForSeconds(0.5f);
            playerCards.Discard(card);
            Debug.Log($"player discards + {card.name}");

        }
        
        foreach (Card card in fateCards.CardsInAction())
        {
            yield return new WaitForSeconds(0.5f);
            fateCards.Discard(card);
            Debug.Log($"fate discards + {card.name}");
        }
        
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<GameStateMaster>().gameState = GameState.GameCleanup;
    }
    
    public void ExecuteCard(Card cardToExecute)
    {
        switch (cardToExecute.cardInfo.cardSuit)
        {
            case Suits.Happy:
                happyMeter.AdjustValue(cardToExecute.cardInfo.cardValue);
                break;
            case Suits.Hunger:
                hungerMeter.AdjustValue(cardToExecute.cardInfo.cardValue);
                break;
            case Suits.Dirty:
                dirtyMeter.AdjustValue(cardToExecute.cardInfo.cardValue);
                break;
            case Suits.Yellow:
                sanityMeter.AdjustValue(cardToExecute.cardInfo.cardValue);
                break;
        }

        Debug.Log($"played + {cardToExecute.name}");
    }
}