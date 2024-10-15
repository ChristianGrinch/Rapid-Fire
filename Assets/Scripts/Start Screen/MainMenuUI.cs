using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
	public static MainMenuUI Instance { get; private set; }

	[Header("Main Menu")]
	public TMP_Text playDefaultText;
	public GameObject difficultySelectWarning;
	public GameObject createSaveWarning;
	public Button playDefaultSave;
	public Button playNewSave;
	public Button quitGame;
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
	// Start is called before the first frame update
	void Start()
	{
		playDefaultText.text = "Play default save \n[ " + GameManager.Instance.defaultSave + " ]";
		playNewSave.onClick.AddListener(() => PopupManager.Instance.ShowPopup(PopupManager.PopupType.CreateSavePopup));
		quitGame.onClick.AddListener(() => GameManager.Instance.QuitGame());
		playDefaultSave.onClick.AddListener(() => {
			ASyncLoader.Instance.LoadLevelBtn("2 - Game");

		});
	}
	public IEnumerator ShowWarning()
	{
		difficultySelectWarning.SetActive(true);
		yield return new WaitForSeconds(2);
		difficultySelectWarning.SetActive(false);

	}
	public IEnumerator ShowSaveNameWarning()
	{
		createSaveWarning.SetActive(true);
		yield return new WaitForSeconds(5);
		createSaveWarning.SetActive(false);

	}
}
