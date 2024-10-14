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
    public string currentSave;
    private List<string> savedGames = new List<string>();
    private List<GameObject> saveButtons = new();
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
                GameManager.Instance.currentSave = btnSaveName;

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
}
