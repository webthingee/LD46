using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI suitText;
    public TextMeshProUGUI valueText;
    public GameObject cardBack;
    public Image cardFaceImage;
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

    public void ShowFront(bool showFront = true)
    {
        cardBack.SetActive(showFront);
    }

    public void UpdateDisplay()
    {
        nameText.text = card.cardInfo.cardName;
        suitText.text = card.cardInfo.cardSuit.ToString();
        valueText.text = card.cardInfo.cardValue.ToString();
        cardFaceImage.sprite = FindObjectOfType<CardFaceMaster>().cardFaceEnumMatch[(int)card.cardInfo.cardSuit];
    }
}