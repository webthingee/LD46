using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public SpriteRenderer bk;
    
    // Start is called before the first frame update
    void Start()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(bk.DOFade(0, 5f))
            .OnComplete(() => LoadMainMenu());
    }

    public void LoadMainMenu()
    {
        SceneKeeper.LoadMainMenuScene();
    }
}
