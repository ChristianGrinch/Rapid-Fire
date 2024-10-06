using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }

	public Slider masterVolumeSlider;
	public Slider musicVolumeSlider;
	public Slider gunVolumeSlider;  
	private float masterVolume = 100;
	private float musicVolume = 100;
	private float gunVolume = 100;

	public AudioClip buttonClickSound;
	private AudioSource buttonAudioSource;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void Start()
	{
		masterVolumeSlider.onValueChanged.AddListener(SetVolume);
		masterVolumeSlider.value = masterVolume;
		SetVolume(masterVolume);

		musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
		musicVolumeSlider.value = musicVolume;
		SetMusicVolume(musicVolume);

		gunVolumeSlider.onValueChanged.AddListener(SetGunVolume);
		gunVolumeSlider.value = gunVolume;
		SetGunVolume(gunVolume);

		InitializeButtonSound();
	}
	private void Update()
	{
		SetVolume(masterVolume);
		SetMusicVolume(musicVolume);
		SetGunVolume(gunVolume);
	}

	public void SetVolume(float newVolume)
	{
		masterVolume = newVolume;

		AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
		foreach (var audioSource in allAudioSources)
		{
			if (!audioSource.CompareTag("Music")) // Only affect non-music audio sources here
			{
				audioSource.volume = masterVolume * 0.01f;
			}
		}
	}

	public void SetMusicVolume(float newVolume)
	{
		musicVolume = newVolume;

		GameObject[] allMusicObjects = GameObject.FindGameObjectsWithTag("Music");
		AudioSource[] allMusicSources = new AudioSource[allMusicObjects.Length];

		for (int i = 0; i < allMusicObjects.Length; i++)
		{
			allMusicSources[i] = allMusicObjects[i].GetComponent<AudioSource>();
		}
		foreach(var audioSource in allMusicSources)
		{
			audioSource.volume = (masterVolume / 100) * (musicVolume / 100);
		}
	}

	public void SetGunVolume(float newVolume)
	{
		gunVolume = newVolume;

		GameObject player = GameObject.Find("Player");
		if(player != null)
		{
			AudioSource audioSource = player.GetComponent<AudioSource>();

			audioSource.volume = (masterVolume / 100) * (gunVolume / 100);
		}
		else
		{
			Debug.LogWarning("Cannot find player!");
		}
		
	}

	void PlayButtonSound()
	{
		buttonAudioSource.PlayOneShot(buttonClickSound);
	}

	void InitializeButtonSound()
	{
		buttonAudioSource = gameObject.AddComponent<AudioSource>();
		buttonAudioSource.clip = buttonClickSound;

		Button[] buttons = FindObjectsOfType<Button>(true);

		foreach (Button btn in buttons)
		{
			if (btn.CompareTag("ButtonWithPop")) { btn.onClick.AddListener(PlayButtonSound); }
		}
	}

    public void AssignSoundToNewButton(GameObject buttonObj)
    {
        Button button = buttonObj.GetComponent<Button>();

        if (button != null)
        {
            button.onClick.AddListener(PlayButtonSound);
        }
        else
        {
            Debug.LogWarning("No Button component found on " + buttonObj.name);
        }
    }
}
