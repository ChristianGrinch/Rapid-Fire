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
	public List<GameObject> HUDSlots = new(5);
	public GameObject slotPrefab;
	public GameObject inventory;
	public GameObject HUD;
	[Header("Render Textures")]
	public RenderTexture pistolRT;
	public RenderTexture assaultRifleRT;
	[Header("Other")]
	public Button button;

	private void Start()
	{
		CreateSlots();
		AssignSlots();
		button.onClick.AddListener(() =>
		{
			slots[4].GetComponent<SlotData>().SetSlotData(0);
			slots[5].GetComponent<SlotData>().SetSlotData(1);
		});
	}
	private void Update()
	{
		DisplayImage();
	}
	void CreateSlots()
	{
		// Create inventory slots
		for (var i = 0; i < slotCount - 5; i++)
		{
			GameObject slot = Instantiate(slotPrefab, inventory.transform);
			slot.name = "Slot " + i;
		}

		// Create HUD slots
		for (var i = 0; i < 5; i++)
		{
			GameObject slot = Instantiate(HUDSlots[i], HUD.transform);
			slot.name = "Slot " + (slotCount - 4 + i); // only minus 4 instead of 5 because the index starts at 0
		}
	}
	void AssignSlots()
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
		foreach (var slot in slots)
		{
			SlotData slotData = slot.GetComponent<SlotData>();
			switch (slotData.itemType)
			{
				case ItemDataType.Gun:
					switch (slotData.gunType)
					{
						case GunType.Pistol:
							slot.GetComponentInChildren<RawImage>().texture = pistolRT;
							Debug.Log("Set slot image to pistol");
							break;
						case GunType.AssaultRifle:
							slot.GetComponentInChildren<RawImage>().texture = assaultRifleRT;
							Debug.Log("Set slot image to assault rifle");
							break;
					}
					break;
				case ItemDataType.Powerup:
					break;
				case ItemDataType.Armor:
					break;
				case ItemDataType.None:
					break;
				default:
					break;
			}
		}
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