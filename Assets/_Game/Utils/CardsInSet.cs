using System;
using System.Collections.Generic;

[Serializable]
public class CardsInSet
{
    public HandRanks handRank;
    public int highValue;
    public List<Card> cardsInSet = new List<Card>();
}