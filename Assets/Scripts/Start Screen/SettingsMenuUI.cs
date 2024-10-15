using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    public static SettingsMenuUI Instance { get; private set; }

    [Header("Settings - Audio Panel")]
    public GameObject audioPanel;
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolume;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolume;
    public Slider gunVolumeSlider;
    public TextMeshProUGUI gunVolume;

    [Header("Settings - Video Panel")]
    public GameObject videoPanel;

    [Header("Settings - Saves Panel")]
    public GameObject savesPanel;
    public GameObject saveButtonPrefab;
    public Transform contentPanel;
    public TMP_InputField saveNameInputField;
    public Button defaultSaveBtn;
    public TMP_Text loadWarning;
    public Button loadSaveBtn;
    public Button deleteSaveBtn;
    public GameObject emptySaveWarning;

    [Header("Saves")]
    public string defaultSaveName;
    private List<string> savedGames = new List<string>();
    private List<GameObject> saveButtons = new();
    [Header("Warnings")]
    public GameObject createSaveWarning;
    public GameObject loadSaveWarning;

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
    public void Start()
    {
        defaultSaveName = SaveSystem.LoadDefaultSave();
        MainMenuUI.Instance.playDefaultText.text = "Play default save \n[ " + defaultSaveName + " ]";
        AddButtonListeners();
        InitializeVolume();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameManager.Instance.isGameUnpaused && StartUIManager.Instance.settingsMenu.activeSelf)
        {
            StartUIManager.Instance.CloseAllMenus();
            StartUIManager.Instance.OpenMainMenu();
        }
    }
    public void InstantiateSaveButtons()
    {
        List<string> saveFiles = SaveSystem.FindSaves();

        foreach (Transform child in contentPanel.transform)
        {
            Destroy(child.gameObject);
        }

        if (saveFiles.Count == 0)
        {
            Debug.Log("No save files to load.");
            return;
        }

        foreach (var save in saveFiles)
        {
            savedGames.Add(save);
            GameObject newButton = Instantiate(saveButtonPrefab, contentPanel);
            newButton.GetComponentInChildren<TMP_Text>().text = save;

            newButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                string btnSaveName = newButton.GetComponentInChildren<TMP_Text>().text;
                UpdateDeleteSaveButton();
                if (!GameManager.Instance.isInGame)
                {
                    SaveManager.Instance.currentSave = btnSaveName;
                }


            });
            // TODO: add audio functionality back later
            //AudioManager.Instance.AssignSoundToNewButton(newButton); 
            newButton.tag = "ButtonWithPop";

            if (save == "") // hacky fix for getting rid of the random empty save file
            {
                Destroy(newButton);
            }
        }

    }
    public void UpdateDeleteSaveButton()
    {
        deleteSaveBtn.gameObject.SetActive(true);
        defaultSaveBtn.gameObject.SetActive(true);
    }
    public void OnSaveButtonClicked()
    {
        string saveName = saveNameInputField.text;

        if (!string.IsNullOrEmpty(saveName) && !SaveSystem.FindSavesBool(saveName))
        {
            CreateNewSave(saveName);
        }
        else
        {
            Debug.LogWarning("Save name cannot be empty OR attempted to create a new save of an already existing name.");
            StartCoroutine(ShowSaveNameWarning());
        }
    }
    public void CreateNewSave(string saveName)
    {
        bool saveNameInSavedGames = false;

        foreach (string savedGame in SaveSystem.FindSaves())
        {
            if (savedGame == saveName)
            {
                saveNameInSavedGames = true;
                break;
            }
        }

        if (!saveNameInSavedGames)
        {
            savedGames.Add(saveName);
            AddButton(saveName);
        }

        SaveManager.Instance.SavePlayer(saveName);
    }
    private void AddButton(string saveName)
    {
        GameObject newButton = Instantiate(saveButtonPrefab, contentPanel);
        newButton.GetComponentInChildren<TMP_Text>().text = saveName;

        Button btn = newButton.GetComponent<Button>();
        btn.onClick.AddListener(() => SaveManager.Instance.LoadPlayer(saveName));
    }
    public void DestroySaveButtons()
    {
        List<string> saveNames = SaveSystem.FindSaves();
        string saveName = saveNameInputField.text;

        for (var i = 0; i < saveNames.Count; i++)
        {
            if (saveName == saveNames[i])
            {
                Destroy(saveButtons[i]);
            }
        }
    }
    public IEnumerator ShowSaveNameWarning()
    {
        createSaveWarning.SetActive(true);
        yield return new WaitForSeconds(5);
        createSaveWarning.SetActive(false);

    }
    public IEnumerator ShowLoadWarning()
    {
        loadSaveWarning.SetActive(true);
        yield return new WaitForSeconds(5);
        loadSaveWarning.SetActive(false);

    }
    void AddButtonListeners()
    {
        deleteSaveBtn.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.DeleteSaveConfirm));
        loadSaveBtn.onClick.AddListener(() =>
        {
            string currentSave = SaveManager.Instance.currentSave;
            if (GameManager.Instance.isInGame)
            {
                Debug.Log("Cannot load save while game is active.");
                loadWarning.gameObject.SetActive(true);
            }
            else
            {
                if (!string.IsNullOrEmpty(currentSave) && SaveSystem.FindSavesBool(currentSave))
                {
                    PopupManager.Instance.ShowPopup(PopupManager.PopupType.PlaySaveConfirm);
                }
                else
                {
                    StartCoroutine(ShowLoadWarning());
                }
            }
        });
    }
    public void InitializeVolume()
    {
        masterVolumeSlider.value = 50;
        musicVolumeSlider.value = 50;
        gunVolumeSlider.value = 35;
        masterVolume.text = masterVolumeSlider.value.ToString();
        musicVolume.text = musicVolumeSlider.value.ToString();
        gunVolume.text = gunVolumeSlider.value.ToString();
    }
    public void DecreaseMasterVolume()
    {
        masterVolumeSlider.value--;
        masterVolume.text = masterVolumeSlider.value.ToString();

    }
    public void IncreaseMasterVolume()
    {
        masterVolumeSlider.value++;
        masterVolume.text = masterVolumeSlider.value.ToString();
    }
    public void DecreaseMusicVolume()
    {
        musicVolumeSlider.value--;
        musicVolume.text = musicVolumeSlider.value.ToString();

    }
    public void IncreaseMusicVolume()
    {
        musicVolumeSlider.value++;
        musicVolume.text = musicVolumeSlider.value.ToString();
    }
    public void DecreaseGunVolume()
    {
        gunVolumeSlider.value--;
        gunVolume.text = gunVolumeSlider.value.ToString();

    }
    public void IncreaseGunVolume()
    {
        gunVolumeSlider.value++;
        gunVolume.text = gunVolumeSlider.value.ToString();
    }
    public void UpdateMasterSlider() 
    { 
        masterVolume.text = masterVolumeSlider.value.ToString();
    }
    public void UpdateMusicSlider() 
    { 
        musicVolume.text = musicVolumeSlider.value.ToString();
    }
    public void UpdateGunSlider() 
    { 
        gunVolume.text = gunVolumeSlider.value.ToString();
    }
}
