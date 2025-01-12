using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
	public static PauseMenuUI Instance { get; private set; }
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
	[Header("Menu")]
	public GameObject pauseMenu;

	[Header("Buttons")]
	public Button quitGame;
	public Button settings;
	public Button returnToStart;
	public Button saveGame;
	public Button returnToGame;
	[Header("Other")]
	public TMP_Text saveGameText;

    private void Start()
    {
		quitGame.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.QuitGameConfirm));
		settings.onClick.AddListener(() => UIManager.Instance.OpenSettings());
        returnToStart.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.StartReturnConfirm));
        saveGame.onClick.AddListener(() => GameManager.Instance.SaveGame(GameManager.Instance.currentSave));
		returnToGame.onClick.AddListener(() => GameManager.Instance.ResumeGame());
    }
}
