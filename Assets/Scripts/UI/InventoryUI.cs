using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
	public static InventoryUI Instance { get; private set; }
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
	public GameObject inventoryMenu;
	[Header("Slots")]
	public List<Button> slots;
	[Header("Other")]
	public bool isInventoryOpen;
	[Header("Tertiary variables")]
	public Vector3 hoveredSlotPos;
	public Vector3 originalSlotPos;
	public int originalSlotNum;
	public int hoveredSlotNum = -1;
	public GameObject grid;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			switch (isInventoryOpen)
			{
				case false:
					OpenInventory();
					isInventoryOpen = true;
					break;
				case true:
					CloseInventory();
					isInventoryOpen = false;
					break;
			}
		}
	}
	public void OpenInventory()
	{
		inventoryMenu.SetActive(true);
	}
	public void CloseInventory()
	{
		inventoryMenu.SetActive(false);
	}
}
