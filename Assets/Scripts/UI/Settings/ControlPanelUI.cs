using TMPro;
using UnityEngine;

public class ControlsPanelUI : MonoBehaviour
{
	public static ControlsPanelUI Instance { get; private set; }
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
	public GameObject controlsPanel;
	[Header("Other")]
	public TMP_Dropdown sprintMode;

	private void Start()
	{

		sprintMode.onValueChanged.AddListener((int value) =>
		{
			SettingsMenuUI.Instance.didModifySettings = true;
			switch (value)
			{
				case 0:
					GameManager.Instance.useSprintHold = true;
					break;
				case 1:
					GameManager.Instance.useSprintHold = false;
					break;
			}
		});
	}
}
