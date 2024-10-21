using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SavesPanelUI : MonoBehaviour
{
	public static SavesPanelUI Instance { get; private set; }
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
	public GameObject savesPanel;
	[Header("Warning")]
	public GameObject createSaveWarning;
	public GameObject loadWarning;
	public GameObject emptyLoadWarning;
	[Header("Buttons")]
	public Button createSave;
	public Button deleteSave;
	public Button defaultSaveBtn;
	public Button loadSave;
	[Header("Save")]
	public string currentSave;
	public string defaultSave;
	[Header("Other")]
	public Transform savesContentPanel;
	public GameObject SavePrefab;
	public TMP_InputField saveNameInput;

	private List<string> savedGames = new();
	private List<GameObject> saveButtons = new();
    private void Update()
    {
        currentSave = GameManager.Instance.currentSave;
    }
    private void Start()
    {
        deleteSave.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.DeleteSaveConfirm));
		defaultSaveBtn.onClick.AddListener(() => GameManager.Instance.SetDefaultSave());
		createSave.onClick.AddListener(() =>
		{
			OnSaveButtonClicked();
			UpdateDeleteSaveButton();
		});
		loadSave.onClick.AddListener(() =>
        {
            if (GameManager.Instance.isInGame)
            {
                Debug.Log("Cannot load save while game is active.");
                loadWarning.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log(currentSave);
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
    public void InstantiateSaveButtons()
	{
		List<string> saveFiles = SaveSystem.FindSaves();

		foreach (Transform child in savesContentPanel.transform)
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
			GameObject newButton = Instantiate(SavePrefab, savesContentPanel);
			newButton.GetComponentInChildren<TMP_Text>().text = save;

			newButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				string btnSaveName = newButton.GetComponentInChildren<TMP_Text>().text;
				UpdateDeleteSaveButton();

				if (!GameManager.Instance.isInGame)
				{
					GameManager.Instance.currentSave = btnSaveName;
				}
			});
			AudioManager.Instance.AssignSoundToNewButton(newButton);
			newButton.tag = "ButtonWithPop";

			if (save == "") // hacky fix for getting rid of the random empty save file
			{
				Destroy(newButton);
			}
		}

	}
	public void DestroySaveButtons()
	{
		List<string> saveNames = SaveSystem.FindSaves();
		string saveName = saveNameInput.text;

		for (var i = 0; i < saveNames.Count; i++)
		{
			if (saveName == saveNames[i])
			{
				Destroy(saveButtons[i]);
			}
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

		GameManager.Instance.SaveGame(saveName);
	}
	private void AddButton(string saveName)
	{
		GameObject newButton = Instantiate(SavePrefab, savesContentPanel);
		newButton.GetComponentInChildren<TMP_Text>().text = saveName;

		Button btn = newButton.GetComponent<Button>();
		btn.onClick.AddListener(() => GameManager.Instance.LoadPlayer(saveName));
	}
	public void OnSaveButtonClicked()
	{
		string saveName = saveNameInput.text;

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
	public void UpdateDeleteSaveButton()
	{
		deleteSave.gameObject.SetActive(true);
		defaultSaveBtn.gameObject.SetActive(true);
	}
    public IEnumerator ShowSaveNameWarning()
    {
        createSaveWarning.SetActive(true);
        yield return new WaitForSeconds(5);
        createSaveWarning.SetActive(false);

    }
    public IEnumerator ShowLoadWarning()
    {
        emptyLoadWarning.SetActive(true);
        yield return new WaitForSeconds(5);
        emptyLoadWarning.SetActive(false);

    }
}
