using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class CharacterUtils : MonoBehaviour
{
    [Button]
    public void AttackThisCharacter()
    {
        CardsInUse[] decks = FindObjectsOfType<CardsInUse>().ToArray();
        
        foreach (CardsInUse deck in decks)
        {
            Card c = deck.allCardsInUse.Find(x => x.cardLocation == CardLocations.inAction);

            if (c != null)
            {
                EvaluateCards e = c.cardInfo.handInUse.actionZone.GetComponent<EvaluateCards>();
                
                GetComponentInChildren<AIHand>().attackedFromZone = e;
                FindObjectOfType<CompareHands>().attacker = e;
                GetComponentInChildren<AIHand>().isDefender = true;
                GetComponentInChildren<AIHand>().Defend();
                Invoke(nameof(CheckForWinner), 2f);
                return;
            }
        }
    }

    private void CheckForWinner()
    {
        FindObjectOfType<CompareHands>().DebugWinner();
    }
}
