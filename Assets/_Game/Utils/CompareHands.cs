using System.Collections.Generic;
using UnityEngine;

public class CompareHands : MonoBehaviour
{
    public EvaluateCards attacker;
    public EvaluateCards defender;
    
    public List<string> winners = new List<string>();

    public void DebugWinner()
    {
        EvaluateCards winner = BetterHandIs();

        int defenderCardsInt = defender.cardsInUse.NumCardsInAction();
        defender.DiscardEvaluatedCards();

        int attackCardsInt = attacker.cardsInUse.NumCardsInAction();
        attacker.DiscardEvaluatedCards();

        if (winner == null)
        {
            winners.Add($"Tie");
            Debug.Log($"Tie");
            defender.cardsInUse.Deal(defenderCardsInt);
            attacker.cardsInUse.Deal(attackCardsInt);
            return;
        }

        winners.Add($"{winner.cardsInUse.name} wins with {winner.handRank}");
        Debug.Log($"{winner.cardsInUse.name} wins with {winner.handRank}");
        
        winner.cardsInUse.Deal(attackCardsInt);

    }

    private EvaluateCards BetterHandIs()
    {
        if (attacker.GetHandRank() > defender.GetHandRank())
        {
            return attacker;
        }

        if (attacker.GetHandRank() == defender.GetHandRank())
        {
            if (attacker.highCardValue > defender.highCardValue)
            {
                return attacker;
            }

            if (attacker.highCardValue == defender.highCardValue)
            {
                return null;
            }
        }
        
        return defender;
    }
}