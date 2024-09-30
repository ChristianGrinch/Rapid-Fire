using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Slider masterVolumeSlider;
    private float volume;
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
        masterVolumeSlider.value = volume;
        SetVolume(volume);
    }
    private void Update()
    {
        SetVolume(volume);
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in allAudioSources)
        {
            audioSource.volume = volume * 0.01f;
        }
    }
}
