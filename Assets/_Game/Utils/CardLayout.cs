using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayout : MonoBehaviour
{
    public float xDistance = 0.25f;
    public CardLocations cardLocation;
    
    public bool alignHorizontal = true;
    public bool alignVerticalDown;
    public bool angleCard;

    public bool isDiscard;
    
    public void MakeChild(Card card)
    {
        card.transform.parent = transform;
        if (isDiscard) Discard(card);
        AlignHorizontal();
    }
    
    public void AlignHorizontal()
    {
        Card[] toAlign = GetComponentsInChildren<Card>();
        int itemsToAlign = toAlign.Length;

        float i = -(itemsToAlign * xDistance - xDistance);
        foreach (Card card in toAlign)
        {
            card.transform.position = new Vector2(i, 0) + (Vector2)transform.position;
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
}
