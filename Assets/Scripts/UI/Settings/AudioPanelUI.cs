using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioPanelUI : MonoBehaviour
{
    public static AudioPanelUI Instance { get; private set; }
    void Awake()
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
    [Header("Panel")]
    public GameObject audioPanel;
    [Header("Sliders")]
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider gunVolume;
    [Header("Volume Text")]
    public TMP_Text master;
    public TMP_Text music;
    public TMP_Text gun;
    [Header("Volume adjustment arrows")]
    public Button masterVolumeIncrease;
    public Button masterVolumeDecrease;
    public Button musicVolumeIncrease;
    public Button musicVolumeDecrease;
    public Button gunVolumeIncrease;
    public Button gunVolumeDecrease;
    void Start()
    {
        masterVolume.onValueChanged.AddListener((value) => UpdateMasterSlider(value));
        musicVolume.onValueChanged.AddListener((value) => UpdateMusicSlider(value));
        gunVolume.onValueChanged.AddListener((value) => UpdateGunSlider(value));

        masterVolumeIncrease.onClick.AddListener(() => ChangeMasterVolume(true));
        masterVolumeIncrease.onClick.AddListener(() => ChangeMasterVolume(false));
        musicVolumeIncrease.onClick.AddListener(() => ChangeMusicVolume(true));
        musicVolumeIncrease.onClick.AddListener(() => ChangeMusicVolume(false));
        gunVolumeIncrease.onClick.AddListener(() => ChangeGunVolume(true));
        gunVolumeIncrease.onClick.AddListener(() => ChangeGunVolume(false));
    }
    public void InitializeVolume()
    {
        masterVolume.value = 50;
        musicVolume.value = 50;
        gunVolume.value = 35;
        master.text = masterVolume.value.ToString();
        music.text = musicVolume.value.ToString();
        gun.text = gunVolume.value.ToString();
    }
    public void UpdateMasterSlider(float value)
    {
        master.text = value.ToString();
    }
    public void UpdateMusicSlider(float value)
    {
        music.text = value.ToString();
    }
    public void UpdateGunSlider(float value)
    {
        gun.text = value.ToString();
    }
    public void ChangeMasterVolume(bool increaseVolume)
    {
        if (increaseVolume)
        {
            masterVolume.value++;
            master.text = masterVolume.value.ToString();
        }
        else
        {
            masterVolume.value--;
            master.text = masterVolume.value.ToString();
        }
    }
    public void ChangeMusicVolume(bool increaseVolume)
    {
        if (increaseVolume)
        {
            musicVolume.value++;
            music.text = musicVolume.value.ToString();
        }
        else
        {
            musicVolume.value--;
            music.text = musicVolume.value.ToString();
        }
    }
    public void ChangeGunVolume(bool increaseVolume)
    {
        if (increaseVolume)
        {
            gunVolume.value++;
            gun.text = gunVolume.value.ToString();
        }
        else
        {
            gunVolume.value--;
            gun.text = gunVolume.value.ToString();
        }
    }

}
