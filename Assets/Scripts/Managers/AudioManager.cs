using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance { get; private set; }
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

	private float masterVolume = 100;
	private float musicVolume = 100;
	private float gunVolume = 100;

	public AudioClip buttonClickSound;
	private AudioSource buttonAudioSource;
	public GameObject player;

	void Start()
	{
		AudioPanelUI.Instance.masterVolume.onValueChanged.AddListener(SetVolume);
		AudioPanelUI.Instance.masterVolume.value = masterVolume;
		SetVolume(masterVolume);

		AudioPanelUI.Instance.musicVolume.onValueChanged.AddListener(SetMusicVolume);
		AudioPanelUI.Instance.musicVolume.value = musicVolume;
		SetMusicVolume(musicVolume);

		AudioPanelUI.Instance.gunVolume.onValueChanged.AddListener(SetGunVolume);
		AudioPanelUI.Instance.gunVolume.value = gunVolume;
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

		AudioSource[] allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
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
		if(player != null)
		{
			AudioSource audioSource = player.GetComponent<AudioSource>();

			audioSource.volume = (masterVolume / 100) * (gunVolume / 100);
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

		Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

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
