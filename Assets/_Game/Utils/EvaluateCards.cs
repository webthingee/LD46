using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EvaluateCards : MonoBehaviour
{
    [HideInInspector] public CardsInUse cardsInUse;
    public HandRanks handRank;
    public int highCardValue;
    public List<CardsInSet> handsToPlay = new List<CardsInSet>();

    private List<Card> _valueList = new List<Card>();

    public void BuildHandEval()
    {
        highCardValue = 0;
        handsToPlay.Clear();
        
        ValueListSetup();
        HighCard();
        Pairs();
        ValueRun();
        SuitRun();

        handRank = DefineHandRank();
    }

    public void DiscardEvaluatedCards()
    {
        foreach (Card card in _valueList)
        {
            card.Discard();
        }
    }

    public List<Card> CardsPlayed(HandRanks handRank)
    {
        List<Card> cardsSentToBattle = new List<Card>();
        CardsInSet cardSet;
        
        switch (handRank)
        {
            case HandRanks.HighCard:
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.HighCard);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);
                break;
            case HandRanks.Pair:
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.Pair);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);
                break;
            case HandRanks.TwoPair:
                //cardsToDestory = bestPair; // ?? hmmm two? might need hign and los pairs instea of best
                break;
            case HandRanks.ThreeOfAKind:
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.ThreeOfAKind);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);                
                break;
            case HandRanks.Straight:
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.Straight);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);                
                break;
            case HandRanks.Flush:
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.Flush);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);                
                break;
            case HandRanks.FullHouse:
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.Pair);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.ThreeOfAKind);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);
                break;
            case HandRanks.FourOfAKind:
                cardSet = handsToPlay.Find(x => x.handRank == HandRanks.FourOfAKind);
                cardsSentToBattle.AddRange(cardSet.cardsInSet);                
                break;
            case HandRanks.StraightFlush:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(handRank), handRank, null);
        }

        return cardsSentToBattle;
    }

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

    public List<Card> ListToEvaluate()
    {
        return GetComponentsInChildren<Card>().OrderBy(x => x.cardInfo.cardValue).ToList();
    }
    
    public void ValueListSetup()
    {
        _valueList.Clear();
        _valueList = ListToEvaluate();
        // set via the last card in the value list (handInUse)
        cardsInUse = _valueList.Last().cardInfo.handInUse;
    }
    
    public void HighCard()
    {
        if (_valueList.Count < 1) return;
        
        highCardValue = _valueList.Last().cardInfo.cardValue;
        
        CardsInSet cs = new CardsInSet();
            cs.handRank = HandRanks.HighCard;
            cs.highValue = _valueList.Last().cardInfo.cardValue;
            cs.cardsInSet.Add(_valueList.Last());
        handsToPlay.Add(cs);
    }

    public void Pairs()
    {
        if (_valueList.Count < 2) return;

        for (int i = 0; i < 14; i++)
        {
            var cardValue = _valueList.FindAll(p => p.cardInfo.cardValue == i).ToArray();
            if (cardValue.Length < 2) continue;

            CardsInSet cs;
            switch (cardValue.Length)
            {
                case 5:
                    //handRankResult.quints.Add(i);
                    continue;                
                case 4:
                    //handRankResult.quads.Add(i);
                    cs = new CardsInSet();
                    cs.handRank = HandRanks.FourOfAKind;
                    cs.highValue = i;
                    foreach (Card card in cardValue)
                    {
                        cs.cardsInSet.Add(card);
                    }
                    handsToPlay.Add(cs);
                    continue;
                case 3:
                    //handRankResult.trips.Add(i);
                    
                    cs = new CardsInSet();
                    cs.handRank = HandRanks.ThreeOfAKind;
                    cs.highValue = i;
                    foreach (Card card in cardValue)
                    {
                        cs.cardsInSet.Add(card);
                    }
                    handsToPlay.Add(cs);
                    continue;
                case 2:
                    //handRankResult.pairs.Add(i);
                    
                    cs = new CardsInSet();
                    cs.handRank = HandRanks.Pair;
                    cs.highValue = i;
                    foreach (Card card in cardValue)
                    {
                        cs.cardsInSet.Add(card);
                    }
                    handsToPlay.Add(cs);
                    continue;
            }
        }
    }

    public void ValueRun()
    {
        if (_valueList.Count < 2) return;
        
        foreach (Card card in _valueList)
        {
            List<Card> runFinder = new List<Card>();
            
            for (int j = 0; j < 7; j++)
            {
                Card runCard = _valueList.Find(p => p.cardInfo.cardValue == card.cardInfo.cardValue + j);
                if (runCard == null) break;
                
                runFinder.Add(runCard);
            }
            
            if (runFinder.Count < 4) continue;
            
            CardsInSet cs = new CardsInSet();
            cs.handRank = HandRanks.Straight;
            cs.highValue = runFinder.Last().cardInfo.cardValue;
            foreach (Card card1 in runFinder)
            {
                cs.cardsInSet.Add(card1);
            }
            handsToPlay.Add(cs);
            
            //handRankResult.valueRun = runFinder.Last().value;
        }
    }
    
    public void SuitRun()
    {
        if (_valueList.Count < 2) return;
        
        foreach (Suits s in Enum.GetValues(typeof(Suits)))
        {
            Card[] cc = _valueList.FindAll(p => p.cardInfo.cardSuit == s).ToArray();
            
            if (cc.Length < 4) continue;
            
            
            CardsInSet cs = new CardsInSet();
            cs.handRank = HandRanks.Flush;
            cs.highValue = cc.Last().cardInfo.cardValue;
            foreach (Card card in cc)
            {
                cs.cardsInSet.Add(card);
            }
            handsToPlay.Add(cs);
            
            //handRankResult.suitRun = cc.Last().value;
        }
    }

    #region ### Define Hands
    public HandRanks DefineHandRank()
    {
        //if (StraightFlush()) return HandRanks.StraightFlush;
        if (FourOfAKind()) return HandRanks.FourOfAKind;
        if (FullHouse()) return HandRanks.FullHouse;
        if (Flush()) return HandRanks.Flush;
        if (Straight()) return HandRanks.Straight;
        if (ThreeOfAKind()) return HandRanks.ThreeOfAKind;
        if (TwoPair()) return HandRanks.TwoPair;
        if (Pair()) return HandRanks.Pair;
        return HandRanks.HighCard;
    }
    
    public bool FourOfAKind()
    {
        return handsToPlay.FindAll(x => x.handRank == HandRanks.FourOfAKind).Count > 0;
    }
    
    public bool FullHouse()
    {
        return handsToPlay.FindAll(x => x.handRank == HandRanks.ThreeOfAKind).Count > 0
            && handsToPlay.FindAll(x => x.handRank == HandRanks.Pair).Count > 0;
    }
    
    public bool Flush()
    {
        return handsToPlay.FindAll(x => x.handRank == HandRanks.Flush).Count > 0;
    }
    
    public bool Straight()
    {
        return handsToPlay.FindAll(x => x.handRank == HandRanks.Straight).Count > 0;
    }
    
    public bool ThreeOfAKind()
    {
        return handsToPlay.FindAll(x => x.handRank == HandRanks.ThreeOfAKind).Count > 0;
    }
    
    public bool TwoPair()
    {
        return handsToPlay.FindAll(x => x.handRank == HandRanks.Pair).Count == 2;
    }
    
    public bool Pair()
    {
        return handsToPlay.FindAll(x => x.handRank == HandRanks.Pair).Count == 1;
    }
    #endregion
    
    #region ### Get Hand Details

    public int GetHandRank()
    {
        BuildHandEval();
        return (int)handRank;
    }
    #endregion
    
    private T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0,A.Length));
        return V;
    }
}