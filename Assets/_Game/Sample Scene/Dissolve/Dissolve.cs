using System.Collections;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    private Material _material;
    private float _fade = 1;
    private bool _isDissolving;

    private void Awake()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    public void DissolveOut()
    {
        StartCoroutine(FadeOut());
    }
    
    public void DissolveIn()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        if (_fade <= 0) yield break;
        
        while (_fade > 0)
        {
            _fade -= Time.deltaTime;
            _material.SetFloat("_Fade", _fade);
            yield return null;
        }

        _fade = 0;
    }
    
    IEnumerator FadeIn()
    {
        if (_fade >= 1) yield break;
        
        while (_fade < 1)
        {
            _fade += Time.deltaTime;
            _material.SetFloat("_Fade", _fade);
            yield return null;
        }

        _fade = 1;
    }
}