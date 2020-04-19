using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class CardLayout : MonoBehaviour
{
    public float xDistance = 0.25f;
    public CardLocations cardLocation;

    public CardLayout validateAgainst;
    
    public bool alignHorizontal = true;
    public bool alignVerticalDown;
    public bool angleCard;

    public int maxCards = 4;
    public bool isDiscard;
    public bool isEndTurn;
    
    public void MakeChild(Card card, bool animate = true)
    {
        Card[] toAlign = GetComponentsInChildren<Card>();
        if (toAlign.Length >= maxCards) return;
        if (card.transform.root != transform.root) return;

        if (validateAgainst != null)
        {
            if (validateAgainst.GetComponentsInChildren<Card>().Length != validateAgainst.maxCards) return;
        }
        
        card.transform.parent = transform;
        if (isEndTurn) EndTurn();
        if (isDiscard) Discard(card);
        AlignHorizontal(animate);
    }
    
    public void AlignHorizontal(bool animate = true)
    {
        Card[] toAlign = GetComponentsInChildren<Card>();
        int itemsToAlign = toAlign.Length;

        float i = -(itemsToAlign * xDistance - xDistance);
        foreach (Card card in toAlign)
        {

            if (!animate)
            {
                card.transform.position = new Vector2(i, 0) + (Vector2)transform.position;
            }
            else
            {
                card.transform.DOMove(new Vector2(i, 0) + (Vector2)transform.position, 0.2f);
            }
            
            i += xDistance * 2;

            //if (angleCard)
            //{
            //    t.transform.DORotate(new Vector3(0, 0, 20), 0.25f);
            //    t.transform.DOScale(0.5f, 0.5f);
            //}
            //else
            //{
            //    t.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
            //}

            int siblingIndex = card.transform.GetSiblingIndex();
            //t.GetComponent<CardDisplayOverride>().AdjustOrder(siblingIndex + 1);
            //CardsMaster.instance.CardCostChanged(gameObject);

            card.cardLocation = cardLocation;
            
            GetComponent<EvaluateCards>()?.BuildHandEval();

        }
    }

    public void Discard(Card card)
    {
        card.GetComponent<Card>().Discard();
    }

    public void EndTurn()
    {
        FindObjectOfType<CompareHands>().ExecutePlayerCards();
    }
}
