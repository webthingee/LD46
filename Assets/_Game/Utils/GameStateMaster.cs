using System.Collections;
using UnityEngine;

public class GameStateMaster : MonoBehaviour
{
    public GameState gameState;

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
        yield return new WaitForSeconds(1f);
        gameState = GameState.FishDraw;


    }
    IEnumerator FishDrawPhase()
    {
        Debug.Log($"Fish Draw Phase");

        foreach (CardsInUse cardsInUse in FindObjectsOfType<CardsInUse>())
        {
            cardsInUse.DealStart();
        }

        yield return new WaitForSeconds(1f);

        //gameState = GameState.FishResolve;
    }
    
    IEnumerator FishResolvePhase()
    {
        Debug.Log($"Fish Resolve Phase");

        while (gameState == GameState.FishResolve)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator PlayerDrawPhase()
    {
        Debug.Log($"Player Draw Phase");

        while (gameState == GameState.PlayerDraw)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
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
        
        while (gameState == GameState.GameCleanup)
        {
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
    }
}
