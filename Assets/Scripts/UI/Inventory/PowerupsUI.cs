using TMPro;
using UnityEngine;

public class PowerupsUI : MonoBehaviour
{
	public static PowerupsUI Instance { get; private set; }
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
	[Header("Slots")]
	public GameObject healthSlot;
	public GameObject ammoSlot;
	public GameObject speedSlot;
	public GameObject tbdSlot;
	[Header("Slot count")]
	public TMP_Text healthCount;
	public TMP_Text speedCount;
	public TMP_Text ammoCount;
	[Header("Slot Data")]
	public SlotData healthSlotData;
	public SlotData ammoSlotData;
	public SlotData speedSlotData;
	private void Start()
	{
		healthSlotData = healthSlot.GetComponent<SlotData>();
		speedSlotData = speedSlot.GetComponent<SlotData>();
		ammoSlotData = ammoSlot.GetComponent<SlotData>();
	}
	public void UpdateCounts()
	{
		speedSlotData.powerupCounts[1] = PlayerController.Instance.speedPowerupCount;
		healthCount.text = healthSlotData.powerupCounts[0].ToString();
		speedCount.text = speedSlotData.powerupCounts[1].ToString();
		ammoCount.text = ammoSlotData.powerupCounts[2].ToString();
	}
}
