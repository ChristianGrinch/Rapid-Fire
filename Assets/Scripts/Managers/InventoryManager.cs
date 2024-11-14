using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SlotData;
public class InventoryManager : MonoBehaviour
{
	public int slotCount = (6 * 4) + 5; // Column x Row inventory slots plus 5 HUD slots
	private List<GameObject> slots;
	[Header("Game objects")]
	public GameObject inventory;
	public GameObject HUD;

	public Button button;

	void Start()
	{
		InitializeSlots();
		button.onClick.AddListener(() =>
		{
			slots[4].GetComponent<SlotData>().SetSlotData(3);
		});
	}
	void InitializeSlots()
	{
		slots = new(slotCount);
		foreach (Transform slot in inventory.transform)
		{
			slots.Add(slot.gameObject);
		}
		foreach (Transform slot in HUD.transform)
		{
			slots.Add(slot.gameObject);
		}
		foreach(var slot in slots)
		{
			slot.GetComponent<SlotData>().NullifyData();
		}
	}
	void DisplayImage()
	{

	}
}
public class InventoryData
{
	static int gunTypesLength = Enum.GetValues(typeof(GunType)).Length;

	List<bool> ownedGuns = new(gunTypesLength);
	List<List<int>> gunUpgrades = new(gunTypesLength);
	List<int> slotData; // 0 represents a gun. 1 represents a powerup. (add more as needed)
	public void SlotData()
	{

	}
}
