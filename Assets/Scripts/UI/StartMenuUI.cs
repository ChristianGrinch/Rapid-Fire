using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public static StartMenuUI Instance { get; private set; }
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
    public GameObject startMenu;
    [Header("Buttons")]
    public Button playDefault;
    public Button createNewSave;
    public Button difficultySelect;
    public Button settingsSelect;
    public Button quitGame;
    [Header("Warnings")]
    public GameObject difficultySelectWarning;
    public GameObject saveNameWarning;
	public GameObject defaultSaveWarning;
    [Header("Other")]
    public TMP_Text playDefaultText;
    public IEnumerator SaveNameWarning()
    {
        saveNameWarning.SetActive(true);
        yield return new WaitForSeconds(5);
        saveNameWarning.SetActive(false);
    }
    public IEnumerator DifficultySelectWarning()
    {
        difficultySelectWarning.SetActive(true);
        yield return new WaitForSeconds(2);
        difficultySelectWarning.SetActive(false);
    }
	public IEnumerator DefaultSaveWarning()
	{
		defaultSaveWarning.SetActive(true);
		yield return new WaitForSeconds(5);
		defaultSaveWarning.SetActive(false);
	}
    private void Start()
    {
		playDefault.onClick.AddListener(() => {
			if (!string.IsNullOrEmpty(GameManager.Instance.defaultSave))
			{
				GameManager.Instance.StartGame();
			}
			else
			{
				StartCoroutine(DefaultSaveWarning());
			}
		});
        createNewSave.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.CreateSavePopup));
        difficultySelect.onClick.AddListener(() => UIManager.Instance.OpenDifficultyScreen());
        settingsSelect.onClick.AddListener(() => UIManager.Instance.OpenSettings());
        quitGame.onClick.AddListener(() => GameManager.Instance.QuitGame());
		if (!string.IsNullOrEmpty(GameManager.Instance.defaultSave))
		{
			playDefaultText.text = "Play default save \n[ " + GameManager.Instance.defaultSave + " ]";
		}
		else
		{
			playDefaultText.text = "Play default save \n[No default save!]";
			Debug.Log("No default save detected.");
		}
    }
}
