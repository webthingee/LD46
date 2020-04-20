using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SplashScreen : MonoBehaviour
{
    public SpriteRenderer bk;
    public AudioClip[] randomSounds;
    // Start is called before the first frame update

    private void Start()
    {
        AudioMaster.PlaySting(randomSounds[Random.Range(0, randomSounds.Length)]);
        
        StartCoroutine(PlayRandomSound());
    }

    IEnumerator PlayRandomSound()
    {
        AudioMaster.PlaySting(randomSounds[Random.Range(0, randomSounds.Length)]);
        
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        
        StartCoroutine(PlayRandomSound());
    }

    public void OnMouseDown()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence
            .Append(bk.DOFade(0, 2.5f))
            .OnComplete(() => LoadMainMenu());
    }

    public void LoadMainMenu()
    {
        SceneKeeper.LoadMainMenuScene();
    }
}
