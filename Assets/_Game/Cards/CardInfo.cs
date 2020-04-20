using UnityEngine;

[System.Serializable]
public class CardInfo
{
    public int cardValue;
    public string cardName;
    public Suits cardSuit;
    public Sprite cardFaceImageOverride;
    public AudioClip audioClip;
    
    [Space(20)][Header("Dynamic")]
    public GameObject cardObj;
    public Deck deck;
    public CardsInUse handInUse;
}