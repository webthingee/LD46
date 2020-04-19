using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

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
        fateAttackCard.GetComponentInChildren<CardUiMover>().isFrozen = true;

        attackArea.MakeChild(fateAttackCard);
    }

    [Button]
    public void ExecuteCard()
    {
        fateAttackCard.transform.DOPunchScale(Vector3.one, 1f);
        fateAttackCard.GetComponentInChildren<CardDisplay>().GetComponent<Image>().DOColor(Color.gray, 1f);

        Vector2 screenPos = fateAttackCard.transform.position;
        GameObject g = Instantiate(FindObjectOfType<CompareHands>().actionShower, screenPos, Quaternion.identity);
        g.name = "g";
        Destroy(g, 2f);

        var r = FindObjectOfType<CompareHands>().happyMeter;
        Vector2 screenPos1 = Camera.main.ScreenToWorldPoint(r.transform.position);
        Sequence mySequence1 = DOTween.Sequence();
        mySequence1
            .Append(g.transform.DOMove(screenPos1, 1f))
            .Join(g.transform.DOScale(Vector3.zero, 1f))
            .OnComplete(() => FindObjectOfType<CompareHands>().ExecuteCard(fateAttackCard));
        
        //FindObjectOfType<CompareHands>().ExecuteCard(fateAttackCard);
    }
}
