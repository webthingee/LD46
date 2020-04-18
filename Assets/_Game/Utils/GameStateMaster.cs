using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class GameStateMaster : MonoBehaviour
{
    public GameState gameState;
    public CardsInUse fateDeck;
    public CardsInUse playerDeck;

    public Button startRoundButton;

    private void Start()
    {
        StartCoroutine(GameStateStatus());
    }

    IEnumerator GameStateStatus()
    {
        yield return StartCoroutine(NonePhase());
        
        yield return StartCoroutine(FishDrawPhase());
        yield return StartCoroutine(FishResolvePhase());
        
        yield return StartCoroutine(PlayerDrawPhase());
        yield return StartCoroutine(PlayerPlacePhase());
        yield return StartCoroutine(PlayerResolvePhase());
        
        yield return StartCoroutine(GameCleanupPhase());
        
        StartCoroutine(GameStateStatus());
    }

    IEnumerator NonePhase()
    {
        Debug.Log($"Entering Phase - None");

        startRoundButton.interactable = true;
        
        yield return new WaitForSeconds(1f);
        
        while (gameState == GameState.None)
        {
            yield return null;
        }
    }

    [Button]
    public void StartRoundButton()
    {
        startRoundButton.interactable = false;
        gameState = GameState.FishDraw;
    }
    
    IEnumerator FishDrawPhase()
    {
        Debug.Log($"Fish Draw Phase");

        fateDeck.Deal(1);
        
        yield return new WaitForSeconds(0.25f);
        fateDeck.GetComponent<AIAttack>().PlayCard();

        yield return new WaitForSeconds(0.25f);
        gameState = GameState.FishResolve;
    }
    
    IEnumerator FishResolvePhase()
    {
        Debug.Log($"Fish Resolve Phase");
        
        fateDeck.GetComponent<AIAttack>().ExecuteCard();

//        while (gameState == GameState.FishResolve)
//        {
//            yield return null;
//        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator PlayerDrawPhase()
    {
        Debug.Log($"Player Draw Phase");

        int toDraw = 5 - playerDeck.CardsInHand().Count;
        playerDeck.Deal(toDraw);

        yield return new WaitForSeconds(1f);
        gameState = GameState.PlayerPlace;

    }
    
    IEnumerator PlayerPlacePhase()
    {
        Debug.Log($"Player Place Phase");
        
        while (gameState == GameState.PlayerPlace)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator PlayerResolvePhase()
    {
        Debug.Log($"Player Resolve Phase");
        
        while (gameState == GameState.PlayerResolve)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator GameCleanupPhase()
    {
        Debug.Log($"Cleanup End of Round");
        
        yield return new WaitForSeconds(1f);
        gameState = GameState.None;
    }
}
