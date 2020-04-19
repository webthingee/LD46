using System;
using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
	public CardData _cardData;
	public CardInfo cardInfo;
	public CardLayout cardLayout;
	public CardLocations cardLocation;

	public event EventHandler OnCardInfoUpdated;

	public void Init(CardData cardData, Deck fromDeck)
	{
		// load the card data
		_cardData = cardData;
		
		// overwrite cardInfo for this instance
		gameObject.name = cardData.name;
		cardInfo = cardData.cardInfo;
		cardInfo.cardObj = gameObject;
		cardInfo.deck = fromDeck;

		OnCardInfoUpdated?.Invoke(this, EventArgs.Empty);
	}

	//public void Draw()
	public IEnumerator Draw()
	{
		cardInfo.handInUse.Draw(this);
		OnCardInfoUpdated?.Invoke(this, EventArgs.Empty);
		yield return new WaitForSeconds(1f);
	}

	public void ShowFront(bool showFront = true)
	{
		GetComponentInChildren<CardDisplay>().ShowFront();
	}
	
	public void Discard()
	{
		cardInfo.handInUse.Discard(this);
	}

	public void ShowFront()
	{
		GetComponentInChildren<CardDisplay>().ShowFront();
	}
}

public enum CardLocations
{
	none,
	outOfPlay,
	discard,
	drawPile,
	inHand,
	inAction
}