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

    // Start is called before the first frame update
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
}
