using UnityEngine;
using UnityEngine.UI;

public class MixLevels : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambientVolumeSlider;
    public Slider sfxVolumeSlider;

    private void OnEnable()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("masterVol", 0);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("ambientVol", 0);
        ambientVolumeSlider.value = PlayerPrefs.GetFloat("musicVol", 0);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("stingVol", 0);
    }

    public void SetMasterLevel(float level)
    {
        AudioMaster.GetAudioMaster().masterGroup.audioMixer.SetFloat("masterVol", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("masterVol", level);
    }
    
    public void SetMusicLevel(float level)
    {
        AudioMaster.GetAudioMaster().musicGroup.audioMixer.SetFloat("musicVol", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("musicVol", level);
    }
    
    public void SetAmbientLevel(float level)
    {
        AudioMaster.GetAudioMaster().ambientGroup.audioMixer.SetFloat("ambientVol", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("ambientVol", level);
    }
    
    public void SetStingLevel(float level)
    {
        AudioMaster.GetAudioMaster().stingGroup.audioMixer.SetFloat("stingVol", Mathf.Log10(level) * 20);
        PlayerPrefs.SetFloat("stingVol", level);
    }
}