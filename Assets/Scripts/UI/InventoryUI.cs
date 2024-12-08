using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SlotData;

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
	public List<GameObject> slots;
	public GameObject heartPowerup;
	public GameObject speedPowerup;
	public GameObject ammoPowerup;

	public TMP_Text heartPowerupCount;
	public TMP_Text speedPowerupCount;
	public TMP_Text ammoPowerupCount;

	[Header("Render Textures")]
	public RenderTexture pistolRT;
	public RenderTexture assaultRifleRT;

	[Header("Other")]
	public bool isInventoryOpen;
	public GameObject invPlayerCamera;

	private void Update()
	{
		//TODO: make only run when the value is updated
		heartPowerupCount.text = heartPowerup.GetComponent<SlotData>().powerupCounts[0].ToString();
		speedPowerupCount.text = speedPowerup.GetComponent<SlotData>().powerupCounts[1].ToString();
		ammoPowerupCount.text = ammoPowerup.GetComponent<SlotData>().powerupCounts[2].ToString();

		//DisplayImage();

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
		invPlayerCamera.transform.position = new(PlayerController.Instance.transform.position.x - 2.5f, PlayerController.Instance.transform.position.y, PlayerController.Instance.transform.position.z + 2.5f);
	}
	public void OpenInventory()
	{
		inventoryMenu.SetActive(true);
	}
	public void CloseInventory()
	{
		inventoryMenu.SetActive(false);
	}
	public void DisplayImage()
	{
		foreach (var slot in slots)
		{
			ItemData itemData = slot.GetComponent<SlotData>().itemData;
			switch (itemData.itemType)
			{
				case ItemDataType.Primary:
					// Add a switch here when more primary types are added, same with secondary and so on
					RawImage rawImage = slot.GetComponentInChildren<RawImage>();
					rawImage.texture = assaultRifleRT;

					Color color = rawImage.color;
					color.a = 1f;
					rawImage.color = color;
					break;
				case ItemDataType.Secondary:
					rawImage = slot.GetComponentInChildren<RawImage>();
					rawImage.texture = pistolRT;

					color = rawImage.color;
					color.a = 1f;
					rawImage.color = color;
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
