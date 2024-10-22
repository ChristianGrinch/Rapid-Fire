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

	private GameObject player;
	private PlayerController playerController;

	private void Start()
	{
		player = GameObject.FindWithTag("Player");
		playerController = player.GetComponent<PlayerController>();

		sprintMode.onValueChanged.AddListener((int value) =>
		{
			switch (value)
			{
				case 0:
					playerController.useSprintHold = true;
					Debug.Log("Sprint mode set to hold");
					break;
				case 1:
					playerController.useSprintHold = false;
					Debug.Log("Sprint mode set to toggle");
					break;
			}
		});
	}
}
