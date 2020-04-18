using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class AIAttack : MonoBehaviour
{
    public CardsInUse fateCards;
    public CardLayout attackArea;

    [Header("Dynamic")]
    public Card fateAttackCard;
    
    public List<Card> ListToOrder(List<Card> cardList)
    {
        return cardList.OrderBy(x => x.cardInfo.cardValue).ToList();
    }

    [Button]
    public void PlayCard()
    {
        if (ListToOrder(fateCards.CardsInHand()).Count < 1) return;
        
        fateAttackCard = ListToOrder(fateCards.CardsInHand()).Last();

        attackArea.MakeChild(fateAttackCard);
    }

    [Button]
    public void ExecuteCard()
    {
        FindObjectOfType<CompareHands>().ExecuteFateCard(fateAttackCard);
    }
}
