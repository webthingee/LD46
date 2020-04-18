using System;
using TMPro;
using UnityEngine;

public class CardDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI suitText;
    public Card card;

    private void Awake()
    {
        card = GetComponentInParent<Card>();
    }

    private void OnEnable()
    {
        if (card != null) card.OnCardInfoUpdated += CardInfoUpdatedReaction;
    }

    private void OnDisable()
    {
        if (card != null) card.OnCardInfoUpdated -= CardInfoUpdatedReaction;
    }
    private void CardInfoUpdatedReaction(object sender, EventArgs e)
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        nameText.text = card.cardInfo.cardName;
        suitText.text = card.cardInfo.cardSuit.ToString();
    }
}