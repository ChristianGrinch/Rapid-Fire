using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public Slider volumeSlider;
    private float volume = 1;
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
        volumeSlider.onValueChanged.AddListener(SetVolume);
        volumeSlider.value = volume;
        SetVolume(volume);
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;

        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (var audioSource in allAudioSources)
        {
            audioSource.volume = volume;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
