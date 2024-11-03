using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SettingsMenuUI : MonoBehaviour
{
    public static SettingsMenuUI Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
			DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [Header("Menu")]
    public GameObject settingsMenu;
	[Header("Buttons")]
	public Button goBack;
	public Button save;
	[Header("Labels")]
	public Button audioLabel;
	public Button videoLabel;
	public Button savesLabel;
	public Button controlsLabel;
	[Header("Other")]
	public bool didModifySettings = false;
	public bool didSaveSettings = false;
	private void Start()
	{
		audioLabel.onClick.AddListener(OpenAudioPanel);
		videoLabel.onClick.AddListener(OpenVideoPanel);
		savesLabel.onClick.AddListener(OpenSavesPanel);
		controlsLabel.onClick.AddListener(OpenControlPanel);
		save.onClick.AddListener(() => 
		{
			didSaveSettings = true;
			GameManager.Instance.SaveSettings();
		});
		goBack.onClick.AddListener(() =>
		{
			if (didModifySettings && !didSaveSettings)
			{
				PopupManager.Instance.ShowPopup(PopupManager.PopupType.QuitWithoutSavingConfirm);
				Debug.Log("ran this bro");
			} 
			else
			{
				if (GameManager.Instance.isInGame)
				{
					UIManager.Instance.CloseAllMenus();
					PauseMenuUI.Instance.pauseMenu.SetActive(true);
				}
				else
				{
					UIManager.Instance.SwitchToStart();
				}
				didSaveSettings = false;
			}
		});
		StartCoroutine(FixModifySettingsOnLoad());
	}
	IEnumerator FixModifySettingsOnLoad()
	{
		yield return new WaitForSeconds(0.05f);
		didModifySettings = false;
	}
	public void CloseAllSettingsPanels() // bad cuz hard coded unlike closeallmenus but i could care less right now
	{
		AudioPanelUI.Instance.audioPanel.SetActive(false);
		VideoPanelUI.Instance.videoPanel.SetActive(false);
		SavesPanelUI.Instance.savesPanel.SetActive(false);
		ControlsPanelUI.Instance.controlsPanel.SetActive(false);
	}
	public void OpenAudioPanel()
	{
		CloseAllSettingsPanels();
		AudioPanelUI.Instance.audioPanel.SetActive(true);
	}
	public void OpenVideoPanel()
	{
		CloseAllSettingsPanels();
		VideoPanelUI.Instance.videoPanel.SetActive(true);
	}
	public void OpenSavesPanel()
	{
		if (!GameManager.Instance.isInGame)
		{
			SavesPanelUI.Instance.InstantiateSaveButtons();
			CloseAllSettingsPanels();
			SavesPanelUI.Instance.savesPanel.SetActive(true);
		}
	}
	public void OpenControlPanel()
	{
		CloseAllSettingsPanels();
		ControlsPanelUI.Instance.controlsPanel.SetActive(true);
	}
}
