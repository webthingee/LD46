using System.Collections;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class CompareHands : MonoBehaviour
{
    public CardsInUse fateCards;
    public CardsInUse playerCards;
    
    public Meter happyMeter;
    public Meter hungerMeter;
    public Meter dirtyMeter;
    public Meter sanityMeter;

    public GameObject actionShower;
    
    [Button]
    public void ExecutePlayerCards()
    {
        StartCoroutine(ExecuteEachCard());
    }

    IEnumerator ExecuteEachCard()
    {
        Card[] playerCardArray = playerCards.CardsInAction().ToArray();
        playerCardArray.Reverse();
        
        foreach (Card card in playerCardArray)
        {
            Debug.Log($"player events for + {card.name}");
            
            Sequence mySequence = DOTween.Sequence();
            mySequence
                .Append(card.transform.DOPunchScale(Vector3.one, 0.5f))
                .Append(card.GetComponentInChildren<CardDisplay>().GetComponent<Image>().DOColor(Color.gray, 0.5f));

            yield return new WaitForSeconds(mySequence.Duration());

            Vector2 screenPos = card.transform.position;
            GameObject g = Instantiate(FindObjectOfType<CompareHands>().actionShower, screenPos, Quaternion.identity);
            Destroy(g, 2f);

            var r = FindObjectOfType<CompareHands>().happyMeter;
            Vector2 screenPos1 = Camera.main.ScreenToWorldPoint(r.transform.position);
            Sequence mySequence1 = DOTween.Sequence();
            mySequence1
                .Append(g.transform.DOMove(screenPos1, 1f))
                .Join(g.transform.DOScale(Vector3.zero, 1f));

            yield return new WaitForSeconds(mySequence1.Duration());
            
            ExecuteCard(card);
        }
        
        foreach (Card card in playerCardArray)
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence
                .Append(card.transform.DOMove(card.cardInfo.handInUse.visualDiscardPile.transform.position, 0.5f))
                .OnComplete(() => playerCards.Discard(card));
            yield return new WaitForSeconds(0.5f);
        }
        
        foreach (Card card in fateCards.CardsInAction())
        {
            Sequence mySequence = DOTween.Sequence();
            mySequence
                .Append(card.transform.DOMove(card.cardInfo.handInUse.visualDiscardPile.transform.position, 0.5f))
                .OnComplete(() => fateCards.Discard(card));
        }

        yield return new WaitForSeconds(0.25f);
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