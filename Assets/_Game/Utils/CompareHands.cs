﻿using System.Collections;
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
    private CompareHands _ch;
    private CardFaceMaster _cfm;
    private void Awake()
    {
        _ch = FindObjectOfType<CompareHands>();
        _cfm = FindObjectOfType<CardFaceMaster>();
    }
    
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
            GameObject g = Instantiate(_ch.actionShower, screenPos, Quaternion.identity);
            Destroy(g, 2f);

            var r = FindObjectOfType<CompareHands>().happyMeter;
            Vector2 screenPos1 = Camera.main.ScreenToWorldPoint(r.transform.position);
            Sequence mySequence1 = DOTween.Sequence();
            mySequence1
                .Append(g.transform.DOMove(screenPos1, 2f))
                .Join(g.transform.DOScale(Vector3.zero, 2f));

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
            case Suits.Hungry:
                hungerMeter.AdjustValue(cardToExecute.cardInfo.cardValue);
                break;
            case Suits.Dirty:
                dirtyMeter.AdjustValue(cardToExecute.cardInfo.cardValue);
                break;
            case Suits.Yellow:
                sanityMeter.AdjustValue(cardToExecute.cardInfo.cardValue);
                break;
        }

        if (cardToExecute.cardInfo.handInUse.isFish)
        {
            _cfm.SadAudio();
        }
        else
        {
            switch (cardToExecute.cardInfo.cardSuit)
            {
                case Suits.Happy:
                    _cfm.HappyAudio();
                    break;
                case Suits.Hungry:
                    _cfm.HungerAudio();
                    break;
                case Suits.Dirty:
                    _cfm.CleanAudio();
                    break;
                case Suits.Yellow:
                    _cfm.HappyAudio();
                    break;
            }
        }
        
        if (_ch.happyMeter.IsMeterFull())
        {
            Debug.Log($"WIN"); 
            SceneKeeper.LoadWinScene();
        }

        if (_ch.hungerMeter.IsMeterFull())
        {
            Debug.Log($"LOSE");  
            SceneKeeper.LoadLoseScene();
        }
        
        if (_ch.dirtyMeter.IsMeterFull())
        {
            Debug.Log($"LOSE"); 
            SceneKeeper.LoadLoseScene();
        }

        Debug.Log($"played + {cardToExecute.name}");
    }
}