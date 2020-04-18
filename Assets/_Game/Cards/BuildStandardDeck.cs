﻿using UnityEditor;
using UnityEngine;

public class BuildStandardDeck : MonoBehaviour
{
    public void CreateMasterDeck()
    {
        for (int i = 2; i < 15; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                CardInfo cardInfo = new CardInfo {cardValue = i};

                switch (j)
                {
                    case 0:
                        cardInfo.cardSuit = Suits.Clubs;
                        break;
                    case 1:
                        cardInfo.cardSuit = Suits.Hearts;
                        break;
                    case 2:
                        cardInfo.cardSuit = Suits.Spades;
                        break;
                    case 3:
                        cardInfo.cardSuit = Suits.Diamonds;
                        break;
                }

                switch (i)
                {
                    case 11:
                        cardInfo.cardName = "Jack";
                        break;
                    case 12:
                        cardInfo.cardName = "Queen";
                        break;
                    case 13:
                        cardInfo.cardName = "King";
                        break;
                    case 14:
                        cardInfo.cardName = "Ace";
                        break;
                    default:
                        cardInfo.cardName = i.ToString();
                        break;
                }

                CardData cd = ScriptableObject.CreateInstance<CardData>();
                cd.cardInfo = cardInfo;

                AssetDatabase.CreateAsset(cd, $"Assets/CardData/{cardInfo.cardName} of {cardInfo.cardSuit}.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}