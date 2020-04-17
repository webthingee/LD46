using DG.Tweening;
using UnityEngine;

public class AudioOnAwake : MonoBehaviour
{
    public AudioClip musicClip;
    public AudioClip ambientClip;
    
    private void Start()
    {
        StartLevelAudio();
    }
    
    public void StartLevelAudio()
    {
        //Set the clip for ambient audio, tell it to loop, and then tell it to play
        AudioMaster.PlayMusic(musicClip);

        //Set the clip for music audio, tell it to loop, and then tell it to play
        AudioMaster.PlayAmbient(ambientClip);
    }

    public void StopMusic(AudioClip clip)
    {
        foreach (AudioSource audioSource in FindObjectsOfType<AudioSource>())
        {
            if (audioSource.clip == clip)
            {
                audioSource.DOFade(0f, 1f).OnComplete(() =>
                {
                    audioSource.Stop();
                    audioSource.volume = 1;
                });
            }
        }
    }
}