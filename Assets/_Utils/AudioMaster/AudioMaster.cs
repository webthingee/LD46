using UnityEngine;
using UnityEngine.Audio;

public class AudioMaster : MonoBehaviour
{
	static AudioMaster current;

    [Header("Ambient Audio")]
    public AudioClip ambientClip;		//The background ambient sound
    public AudioClip musicClip;			//The background music 

	[Header("Stings")]
	public AudioClip cardFlip;			//The sting played when the scene loads
	public AudioClip cardValid;			//The sting played when the scene loads
	public AudioClip startLevel;		//The sting played when the player dies
	public AudioClip messageFlyIn; 		//The sting played when the door opens

	[Header("Mixer Groups")]
	public AudioMixerGroup masterGroup;	//The ambient mixer group
	public AudioMixerGroup musicGroup;  //The music mixer group
	public AudioMixerGroup ambientGroup;//The ambient mixer group
	public AudioMixerGroup stingGroup;  //The sting mixer group
	public AudioMixerGroup playerGroup; //The player mixer group
	public AudioMixerGroup voiceGroup;  //The voice mixer group

	AudioSource ambientSource;			//Reference to the generated ambient Audio Source
    AudioSource musicSource;            //Reference to the generated music Audio Source
	AudioSource stingSource;            //Reference to the generated sting Audio Source
	AudioSource playerSource;           //Reference to the generated player Audio Source
	AudioSource voiceSource;            //Reference to the generated voice Audio Source

	void Awake()
	{
		if (current == null)
		{
			current = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (current != this)
		{
			Debug.Log("not the instance " + name);
			Destroy(gameObject);
		}

		//Generate the Audio Source "channels" for our game's audio
        musicSource		= gameObject.AddComponent<AudioSource>() as AudioSource;
		ambientSource	= gameObject.AddComponent<AudioSource>() as AudioSource;
        stingSource		= gameObject.AddComponent<AudioSource>() as AudioSource;
        playerSource	= gameObject.AddComponent<AudioSource>() as AudioSource;
        voiceSource		= gameObject.AddComponent<AudioSource>() as AudioSource;

		//Assign each audio source to its respective mixer group so that it is
		//routed and controlled by the audio mixer
		musicSource.outputAudioMixerGroup	= musicGroup;
		ambientSource.outputAudioMixerGroup = ambientGroup;
		stingSource.outputAudioMixerGroup	= stingGroup;
		playerSource.outputAudioMixerGroup	= playerGroup;
		voiceSource.outputAudioMixerGroup	= voiceGroup;
	}

	public static AudioMaster GetAudioMaster()
	{
		return current;
	}

	private void OnEnable()
	{
		GetMixerLevelsFromPlayerPrefs();
	}

	public void GetMixerLevelsFromPlayerPrefs()
	{		
		float masterLvl = PlayerPrefs.GetFloat("masterVol", 1f);
		//masterGroup.audioMixer.SetFloat("masterVol", masterLvl);
		masterGroup.audioMixer.SetFloat("masterVol", Mathf.Log10(masterLvl) * 20);

		float musicLvl = PlayerPrefs.GetFloat("musicVol", 1f);
		musicGroup.audioMixer.SetFloat("musicVol", Mathf.Log10(musicLvl) * 20);
		
		float ambientLvl = PlayerPrefs.GetFloat("ambientVol", 1f);
		ambientGroup.audioMixer.SetFloat("ambientVol", Mathf.Log10(ambientLvl) * 20);
		
		float stingLvl = PlayerPrefs.GetFloat("stingVol", 1f);
		stingGroup.audioMixer.SetFloat("stingVol", Mathf.Log10(stingLvl) * 20);
		voiceGroup.audioMixer.SetFloat("stingVol", Mathf.Log10(stingLvl) * 20);
		playerGroup.audioMixer.SetFloat("stingVol", Mathf.Log10(stingLvl) * 20);
	}

	public static void PlayMusic(AudioClip musicClip, bool isLooping = true)
	{
		//Set the clip for music audio, tell it to loop, and then tell it to play
		current.musicSource.clip = musicClip;
		current.musicSource.loop = isLooping;
		current.musicSource.Play();
		
		//? how do I stop/remove? Might need to keep a list.
	}
	
	public static void PlayAmbient(AudioClip ambientClip, bool isLooping = true)
	{
		//Set the clip for music audio, tell it to loop, and then tell it to play
		current.ambientSource.clip = ambientClip;
		current.ambientSource.loop = isLooping;
		current.ambientSource.Play();
		
		//? how do I stop/remove? Might need to keep a list.
	}
	
	public static void PlaySting(AudioClip audioClip)
	{
		//If there is no current AudioManager, exit
		if (current == null)
			return;

		//Set the jump SFX clip and tell the source to play
		current.stingSource.clip = audioClip;
		current.stingSource.Play();
	}
	
	public static void PlayPlayer(AudioClip audioClip)
	{
		//If there is no current AudioManager, exit
		if (current == null)
			return;

		//Set the jump SFX clip and tell the source to play
		current.playerSource.clip = audioClip;
		current.playerSource.Play();
	}
}
