using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFaceMaster : MonoBehaviour
{
    public AudioClip[] cardFlips;
    public Sprite[] cardFaceEnumMatch;
    public AudioClip[] cardFaceEnumAudio;
    public AudioClip[] happyAudio;
    public AudioClip[] sadAudio;
    public AudioClip[] cardDrops;
    public AudioClip[] hungerAudio;
    public AudioClip[] cleanAudio;

    public void CardFlip()
    {
        AudioMaster.PlayPlayer(cardFlips[Random.Range(0, cardFlips.Length)]);
    }
    
    public void CardDrop()
    {
        AudioMaster.PlayPlayer(cardDrops[Random.Range(0, cardDrops.Length)]);
    }
    
    public void HappyAudio()
    {
        AudioMaster.PlaySting(happyAudio[Random.Range(0, happyAudio.Length)]);
    }
    
    public void SadAudio()
    {
        AudioMaster.PlaySting(sadAudio[Random.Range(0, sadAudio.Length)]);
    }
    
    public void HungerAudio()
    {
        AudioMaster.PlaySting(hungerAudio[Random.Range(0, hungerAudio.Length)]);
    }
    
    public void CleanAudio()
    {
        AudioMaster.PlaySting(cleanAudio[Random.Range(0, cleanAudio.Length)]);
    }
}
