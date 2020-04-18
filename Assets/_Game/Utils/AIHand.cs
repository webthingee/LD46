using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIHand : MonoBehaviour
{
    public EvaluateCards handEvaluation;
    public EvaluateCards attackedFromZone;
    public bool isDefender;
    public int defenseMaxInt = 2;

    private void Awake()
    {
        handEvaluation = GetComponent<EvaluateCards>();
    }

    public void Defend()
    {
        isDefender = true;
        FindObjectOfType<CompareHands>().defender = handEvaluation.cardsInUse.actionZone.GetComponent<EvaluateCards>();
        Debug.Log($"Attacker played {attackedFromZone.cardsInUse.NumCardsInAction()} cards");
        int numCardsToMatch = attackedFromZone.cardsInUse.NumCardsInAction();

        switch (numCardsToMatch)
        {
            case 1:
                PlayOneCard();
                break;
            case 2:
                PlayTwoCards();
                break;
            case 3:
                PlayThreeCards();
                break;
            case 4:
                PlayFourCards();
                break;
        }
    }

    public void Attack()
    {
        isDefender = false;
        FindObjectOfType<CompareHands>().attacker = handEvaluation.cardsInUse.actionZone.GetComponent<EvaluateCards>();

        switch (handEvaluation.handRank)
        {
            case HandRanks.HighCard:
                PlayOneCard();
                break;
            case HandRanks.Pair:
                PlayTwoCards();
                break;
            case HandRanks.TwoPair:
                PlayFourCards();
                break;
            case HandRanks.ThreeOfAKind:
                PlayThreeCards();
                break;
            case HandRanks.Straight:
                PlayFourCards();
                break;
            case HandRanks.Flush:
                PlayFourCards();
                break;
            case HandRanks.FullHouse:
                PlayThreeCards();
                break;
            case HandRanks.FourOfAKind:
                PlayFourCards();
                break;
            case HandRanks.StraightFlush:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void PlayCard(Card card)
    {
        handEvaluation.cardsInUse.actionZone.MakeChild(card.transform);
        Debug.Log($"{handEvaluation.name} plays {card.cardInfo.cardValue} of {card.cardInfo.cardSuit}'s");
    }

    private void PlayCards(List<Card> cardsToPlay)
    {
        foreach (Card card in cardsToPlay)
        {
            PlayCard(card);
        }
    }
    
    public void PlayOneCard()
    {
        if (isDefender)
        {
            PlayCard(FindHigherCard(true));
        }
        else
        {
            PlayCard(FindHigherCard());
        }
    }

    public void PlayTwoCards()
    {
        List<Card> playPair = new List<Card>();

        if (isDefender)
        {
            playPair.AddRange(FindBestPair(handEvaluation.highCardValue));
            Debug.Log($"{handEvaluation.highCardValue}");
            
            if (playPair.Count > 0)
            {
                Debug.Log($"{gameObject.name} is playing acceptable pair");
                PlayCards(playPair);
            }
            else
            {
                Debug.Log($"{gameObject.name} is playing two low cards");
                for (int i = 0; i < 2; i++)
                {
                    Card lowestCard = FindHigherCard(true);
                    PlayCard(lowestCard);
                }
            }
        }
        else
        {
            playPair.AddRange(FindBestPair());
            
            if (playPair.Count > 0)
            {
                Debug.Log($"{gameObject.name} is playing acceptable pair");
                PlayCards(playPair);
            }
            else
            {
                Debug.Log($"{gameObject.name} error playing pair, will play one card");
                PlayOneCard();
            }
        }
    }
    
    public void PlayThreeCards()
    {
        List<Card> playThree = new List<Card>();

        if (isDefender)
        {
            playThree = FindBestThreeOfAKind(handEvaluation.highCardValue);
            Debug.Log($"{handEvaluation.highCardValue}");
        
            if (playThree != null)
            {
                PlayCards(playThree);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Card lowestCard = FindHigherCard(true);
                    PlayCard(lowestCard);
                }
            }
        }
        else
        {
            playThree = FindBestThreeOfAKind();
        
            if (playThree.Count > 0)
            {
                PlayCards(playThree);
            }
            else
            {
                Debug.Log($"{gameObject.name} error playing trips, will play one card");
                PlayOneCard();
            }
        }
    }

    public void PlayFourCards()
    {
        List<Card> playFour = new List<Card>();

        if (isDefender)
        {
            if (attackedFromZone.handRank == HandRanks.TwoPair)
            {
                playFour = FindBestTwoPair(handEvaluation.highCardValue);
                if (playFour != null)
                {
                    playFour = FindBestStraight(handEvaluation.highCardValue);
                    if (playFour != null)
                    {
                        playFour = FindBestFlush(handEvaluation.highCardValue);
                        if (playFour != null)
                        {
                            playFour = FindBestFourOfAKind(handEvaluation.highCardValue);
                        }
                    }
                }
            }
            
            if (attackedFromZone.handRank == HandRanks.Straight)
            {
                playFour = FindBestStraight(handEvaluation.highCardValue);
                if (playFour != null)
                {
                    playFour = FindBestFlush(handEvaluation.highCardValue);
                    if (playFour != null)
                    {
                        playFour = FindBestFourOfAKind(handEvaluation.highCardValue);
                    }
                }
            }
            
            if (attackedFromZone.handRank == HandRanks.Flush)
            {
                playFour = FindBestFlush(handEvaluation.highCardValue);
                if (playFour.Count <= 0)
                {
                    playFour = FindBestFourOfAKind(handEvaluation.highCardValue);
                }
            } 
            
            if (attackedFromZone.handRank == HandRanks.FourOfAKind)
            {
                playFour = FindBestFourOfAKind(handEvaluation.highCardValue);
            }   
            
            Debug.Log($"{handEvaluation.highCardValue}");
        
            if (playFour != null)
            {
                PlayCards(playFour);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    Card lowestCard = FindHigherCard(true);
                    PlayCard(lowestCard);
                }
            }
        }
        else
        {
            if (handEvaluation.handRank == HandRanks.TwoPair)
            {
                playFour = FindBestTwoPair();
            }
            
            if (handEvaluation.handRank == HandRanks.FourOfAKind)
            {
                playFour = FindBestFourOfAKind();
            }
        
            if (playFour.Count > 0)
            {
                PlayCards(playFour);
            }
            else
            {
                Debug.Log($"{gameObject.name} error playing trips, will play one card");
                PlayOneCard();
            }
        }
    }

    public Card FindHigherCard(bool isDefending = false)
    {
        // new list in value order
        List<Card> cardsValueOrder = handEvaluation.cardsInUse.CardsInHand().OrderBy(x => x.cardInfo.cardValue).ToList();

        // is there a card of equal value
        if (isDefending)
        {
            // card to beat
            int valueToBeat = attackedFromZone.cardsInUse.CardsInAction()[0].cardInfo.cardValue; //cardsInHand[0].value;
            
            Card card = cardsValueOrder.Find(x => x.cardInfo.cardValue == valueToBeat);
            
            // nope, check for next highest value, that is within the defense tolerance limit set by the defender
            if (card == null)
            {
                foreach (Card card1 in cardsValueOrder)
                {
                    //Debug.Log($"{card1.value} - {valueToBeat} = {card1.value - valueToBeat} <= {defenseMaxInt} ; && {card1.value - valueToBeat} >= 0");
                    if (card1.cardInfo.cardValue - valueToBeat <= defenseMaxInt && card1.cardInfo.cardValue - valueToBeat >= 0)
                    {
                        card = card1;
                        Debug.Log($"within def max + {card1.cardInfo.cardValue} {card1.cardInfo.cardSuit}'s");
                        return card;
                    }
                }
            }

            if (card == null)
            {
                card = cardsValueOrder.First();
            }
            
            Debug.Log($"equal or lower + {card.cardInfo.cardValue} {card.cardInfo.cardSuit}'s");
            return card;
            
        }
        else
        {
            Card card = cardsValueOrder.Last();
            Debug.Log($"highest possible + {card.cardInfo.cardValue} {card.cardInfo.cardSuit}'s");
            return card;
        }
    }
    
    public Card FindLowestCard(bool isDefending = false)
    {
        // new list in value order
        List<Card> cardsValueOrder = handEvaluation.cardsInUse.CardsInHand().OrderBy(x => x.cardInfo.cardValue).ToList();
        Card card = cardsValueOrder.First();

        Debug.Log($"highest possible + {card.cardInfo.cardValue} {card.cardInfo.cardSuit}'s"); 
        return card;
    }

    public void PlayBestHand()
    {
        if (!isDefender)
        {
            PlayCards(FindHighestHand());
        }
        else
        {
            PlayCards(FindHighestHand());
            PlayCards(FindHighestHand());
        }
    }
    
    public List<Card> FindHighestHand()
    {
        return handEvaluation.CardsPlayed(handEvaluation.handRank);
    }
    
    public List<Card> FindBestPair(int matchValue = 0)
    {
        // @TODO does this avoid breaking up a low pair?
        List<Card> returnPair = new List<Card>();
        List<CardsInSet> pairs = handEvaluation.handsToPlay.FindAll(x => x.handRank == HandRanks.Pair).ToList();

        if (matchValue == 0)
        {
            returnPair = pairs.Last().cardsInSet;
            return returnPair;
        }
        
        //CardsInSet usablePair = pair.Find(x => x.highValue >= matchValue);
        if (pairs.Count == 0) return returnPair;
        
        foreach (CardsInSet cardsInSet in pairs)
        {
            //Debug.Log($"{card1.value} - {valueToBeat} = {card1.value - valueToBeat} <= {defenseMaxInt} ; && {card1.value - valueToBeat} >= 0");
            if (cardsInSet.highValue - matchValue <= defenseMaxInt && cardsInSet.highValue - matchValue >= 0)
            {
                returnPair = cardsInSet.cardsInSet;
            }
        }

        return returnPair;
    }
    
    public List<Card> FindBestThreeOfAKind(int matchValue = 0)
    {
        // @TODO does this avoid braking up a low pair or a low high ranking hand
        List<Card> returnThreeCardCombo = new List<Card>();
        List<CardsInSet> trips = handEvaluation.handsToPlay.FindAll(x => x.handRank == HandRanks.ThreeOfAKind).ToList();

        if (matchValue == 0)
        {
            returnThreeCardCombo = trips.Last().cardsInSet;
            return returnThreeCardCombo;
        }
        
        CardsInSet usableTrips = trips.Find(x => x.highValue >= matchValue);

        if (usableTrips == null) return null;

        foreach (Card card in usableTrips.cardsInSet)
        {
            returnThreeCardCombo.Add(card);
        }

        return returnThreeCardCombo;
    }
    
    public List<Card> FindBestTwoPair(int matchValue = 0)
    {
        // @TODO does this avoid braking up a low pair or a low high ranking hand
        List<Card> returnTwoPair = new List<Card>();
        List<CardsInSet> pairs = handEvaluation.handsToPlay.FindAll(x => x.handRank == HandRanks.Pair).ToList();

        if (matchValue == 0)
        {
            returnTwoPair.AddRange(pairs.First().cardsInSet);
            returnTwoPair.AddRange(pairs.Last().cardsInSet);
            return returnTwoPair;
        }
        
        CardsInSet usable = pairs.Find(x => x.highValue >= matchValue);

        if (usable == null) return null;

        returnTwoPair.AddRange(usable.cardsInSet);
        returnTwoPair.AddRange(pairs.Last().cardsInSet);

        return returnTwoPair;
    }
    
    public List<Card> FindBestFourOfAKind(int matchValue = 0)
    {
        // @TODO does this avoid braking up a low pair or a low high ranking hand
        List<Card> returnFour = new List<Card>();
        List<CardsInSet> fourOfAKind = handEvaluation.handsToPlay.FindAll(x => x.handRank == HandRanks.FourOfAKind).ToList();

        if (matchValue == 0)
        {
            returnFour = fourOfAKind.Last().cardsInSet;
            return returnFour;
        }
        
        CardsInSet usable = fourOfAKind.Find(x => x.highValue >= matchValue);

        if (usable == null) return null;

        returnFour = usable.cardsInSet;
            
        return returnFour;
    }

    public List<Card> FindBestStraight(int matchValue = 0)
    {
        List<Card> returnFiveCardCombo = new List<Card>();
        List<CardsInSet> straight = handEvaluation.handsToPlay.FindAll(x => x.handRank == HandRanks.Straight).ToList();

        if (matchValue == 0)
        {
            returnFiveCardCombo = straight.Last().cardsInSet;
            return returnFiveCardCombo;
        }
    
        CardsInSet usableStraight = straight.Find(x => x.highValue >= matchValue);

        if (usableStraight == null) return null;

        foreach (Card card in usableStraight.cardsInSet)
        {
            returnFiveCardCombo.Add(card);
        }

        return returnFiveCardCombo;
    }
    
    public List<Card> FindBestFlush(int matchValue = 0)
    {
        List<Card> returnFiveCardCombo = new List<Card>();
        List<CardsInSet> flush = handEvaluation.handsToPlay.FindAll(x => x.handRank == HandRanks.Flush).ToList();

        if (matchValue == 0)
        {
            returnFiveCardCombo = flush.Last().cardsInSet;
            return returnFiveCardCombo;
        }
    
        CardsInSet usableStraight = flush.Find(x => x.highValue >= matchValue);

        if (usableStraight == null) return null;

        foreach (Card card in usableStraight.cardsInSet)
        {
            returnFiveCardCombo.Add(card);
        }

        return returnFiveCardCombo;
    }
    
}
